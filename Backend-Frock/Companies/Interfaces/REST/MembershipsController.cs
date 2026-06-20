using Backend_Frock.Companies.Domain.Model.Commands;
using Backend_Frock.Companies.Domain.Model.Queries;
using Backend_Frock.Companies.Domain.Services;
using Backend_Frock.Companies.Interfaces.REST.Resources;
using Backend_Frock.Companies.Interfaces.REST.Transform;
using Backend_Frock.IAM.Application.OutboundServices;
using Backend_Frock.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using Backend_Frock.IAM.Interfaces.ACL;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace Backend_Frock.Companies.Interfaces.REST
{
    [ApiController]
    [Route("api/memberships")]
    [Authorize]
    [Produces(MediaTypeNames.Application.Json)]
    [Tags("Memberships")]
    [SwaggerTag("Endpoints for company memberships")]
    public class MembershipsController(
        ICompanyMembershipCommandService commandService,
        ICompanyMembershipQueryService queryService,
        ICompanyQueryService companyQueryService,
        ITokenService tokenService,
        IIamContextFacade iamContextFacade) : ControllerBase
    {
        // Lee el userId del JWT igual que SubscriptionsController.
        private async Task<int?> GetCurrentUserId()
        {
            var token = Request.Headers["Authorization"]
                .FirstOrDefault()?.Split(" ").Last();
            return await tokenService.ValidateToken(token ?? string.Empty);
        }

        /// <summary>Gets the membership of the current user.</summary>
        [HttpGet("me")]
        [SwaggerOperation(Summary = "Gets my membership", OperationId = "GetMyMembership")]
        [SwaggerResponse(StatusCodes.Status200OK, "The current membership.", typeof(MyMembershipResource))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "The user has no membership.")]
        public async Task<IActionResult> GetMyMembership()
        {
            var userId = await GetCurrentUserId();
            if (userId is null) return Unauthorized(new { message = "Token inválido." });

            var membership = await queryService.Handle(new GetMembershipByUserIdQuery(userId.Value));
            if (membership is null)
                return NotFound(new { message = "No perteneces a ninguna empresa." });

            var company = await companyQueryService.Handle(new GetCompanyByIdQuery(membership.CompanyId));
            if (company is null)
                return NotFound(new { message = "Empresa no encontrada." });

            return Ok(MembershipResourceFromEntityAssembler.ToMyResourceFromEntity(membership, company));
        }

        /// <summary>Joins a company using an invitation code.</summary>
        [HttpPost("join")]
        [SwaggerOperation(Summary = "Joins a company with an invitation code", OperationId = "JoinCompany")]
        [SwaggerResponse(StatusCodes.Status200OK, "Joined the company.", typeof(MembershipResource))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Invalid invitation code.")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "The user already belongs to a company.")]
        public async Task<IActionResult> JoinCompany([FromBody] JoinCompanyResource resource)
        {
            var userId = await GetCurrentUserId();
            if (userId is null) return Unauthorized(new { message = "Token inválido." });

            try
            {
                var membership = await commandService.Handle(
                    new JoinCompanyCommand(userId.Value, resource.InvitationCode));
                if (membership is null) return BadRequest(new { message = "No se pudo unir a la empresa." });

                var company = await companyQueryService.Handle(new GetCompanyByIdQuery(membership.CompanyId));
                return Ok(MembershipResourceFromEntityAssembler.ToResourceFromEntity(membership, company!));
            }
            catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
            catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
        }

        /// <summary>Leaves the current company (drivers only).</summary>
        [HttpDelete("me")]
        [SwaggerOperation(Summary = "Leaves my company", OperationId = "LeaveCompany")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Left the company.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "The user has no membership.")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "An admin cannot leave the company.")]
        public async Task<IActionResult> LeaveCompany()
        {
            var userId = await GetCurrentUserId();
            if (userId is null) return Unauthorized(new { message = "Token inválido." });

            try
            {
                await commandService.Handle(new LeaveCompanyCommand(userId.Value));
                return NoContent();
            }
            catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
            catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
        }

        /// <summary>Lists the members of a company (admin only).</summary>
        [HttpGet("company/{companyId}")]
        [SwaggerOperation(Summary = "Lists company members", OperationId = "ListCompanyMembers")]
        [SwaggerResponse(StatusCodes.Status200OK, "The list of members.", typeof(IEnumerable<MembershipResource>))]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Only the company admin can list members.")]
        public async Task<IActionResult> ListCompanyMembers(int companyId)
        {
            var userId = await GetCurrentUserId();
            if (userId is null) return Unauthorized(new { message = "Token inválido." });

            var requester = await queryService.Handle(new GetMembershipByUserIdQuery(userId.Value));
            if (requester is null || !requester.IsAdmin || requester.CompanyId != companyId)
                return StatusCode(StatusCodes.Status403Forbidden,
                    new { message = "Solo el administrador puede listar los miembros." });

            var company = await companyQueryService.Handle(new GetCompanyByIdQuery(companyId));
            if (company is null) return NotFound(new { message = "Empresa no encontrada." });

            var members = await queryService.Handle(new GetMembersByCompanyIdQuery(companyId));
            var resources = new List<MembershipResource>();
            foreach (var member in members)
            {
                // Cross into IAM via the ACL facade to enrich the username (DDD-friendly).
                var username = await iamContextFacade.FetchUsernameByUserId(member.UserId);
                resources.Add(MembershipResourceFromEntityAssembler.ToResourceFromEntity(
                    member, company, string.IsNullOrEmpty(username) ? null : username));
            }
            return Ok(resources);
        }

        /// <summary>Removes a member from the company (admin only).</summary>
        [HttpDelete("{membershipId}")]
        [SwaggerOperation(Summary = "Removes a company member", OperationId = "RemoveMember")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "The member was removed.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Only the company admin can remove members.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "The member was not found.")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "The member cannot be removed.")]
        public async Task<IActionResult> RemoveMember(int membershipId)
        {
            var userId = await GetCurrentUserId();
            if (userId is null) return Unauthorized(new { message = "Token inválido." });

            try
            {
                await commandService.Handle(new RemoveMemberCommand(userId.Value, membershipId));
                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
            }
            catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
            catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
        }

        /// <summary>Regenerates the invitation code of a company (admin only).</summary>
        [HttpPost("company/{companyId}/invitation-code/regenerate")]
        [SwaggerOperation(Summary = "Regenerates the invitation code", OperationId = "RegenerateInvitationCode")]
        [SwaggerResponse(StatusCodes.Status200OK, "The new invitation code.", typeof(CompanyResource))]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Only the company admin can regenerate the code.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "The company was not found.")]
        public async Task<IActionResult> RegenerateInvitationCode(int companyId)
        {
            var userId = await GetCurrentUserId();
            if (userId is null) return Unauthorized(new { message = "Token inválido." });

            try
            {
                var company = await commandService.Handle(
                    new RegenerateInvitationCodeCommand(userId.Value, companyId));
                return Ok(CompanyResourceFromEntityAssembler.ToResourceFromEntity(company!));
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
            }
            catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        }
    }
}
