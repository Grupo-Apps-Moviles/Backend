using Backend_Frock.Stops.Domain.Model.Commands;
using Backend_Frock.Stops.Interfaces.REST.Resources;

namespace Backend_Frock.Stops.Interfaces.REST.Transform
{
    public class DeleteStopCommandFromResourceAssembler
    {
        public static DeleteStopCommand ToCommandFromResource(DeleteStopResource resource)
        {
            return new DeleteStopCommand(resource.Id);
        }

    }
}
