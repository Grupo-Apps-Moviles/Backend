-- ============================================================================
--  Fase 1 — CompanyMembership: SCHEMA DELTA + BACKFILL para MySQL (Railway)
-- ============================================================================
--  IMPORTANTE
--  La base de datos de Railway se creó con `Database.EnsureCreated()` y NUNCA
--  ha usado migraciones EF. Por eso la migración generada
--  (`AddCompanyMembershipAndInvitationCode`) es una migración INICIAL que crea
--  TODO el esquema. NO ejecutes `dotnet ef database update` contra Railway:
--  intentaría recrear tablas que ya existen y fallará.
--
--  En su lugar, aplica manualmente este script:
--    Parte 1) crea SOLO lo nuevo (tabla company_memberships + columna
--             invitation_code en companies, con sus índices únicos).
--    Parte 2) backfill de datos para las compañías ya existentes.
--
--  Ejecuta el script COMPLETO una sola vez. Es idempotente en lo razonable
--  (usa IF NOT EXISTS y comprobaciones), pero revisa antes de correrlo.
--  Nombres de columnas verificados contra la migración EF real (snake_case).
-- ============================================================================

-- ----------------------------------------------------------------------------
-- PARTE 1 — SCHEMA DELTA (solo lo nuevo de esta fase)
-- ----------------------------------------------------------------------------

-- 1.1) Columna invitation_code en companies (varchar(12), NOT NULL)
--      Se agrega permitiendo NULL temporalmente para poder backfillear,
--      y luego se vuelve NOT NULL + índice único en la Parte 2.
ALTER TABLE companies
    ADD COLUMN IF NOT EXISTS invitation_code VARCHAR(12) NULL;

-- 1.2) Tabla company_memberships
CREATE TABLE IF NOT EXISTS company_memberships (
    id           INT          NOT NULL AUTO_INCREMENT,
    company_id   INT          NOT NULL,
    user_id      INT          NOT NULL,
    member_role  INT          NOT NULL,   -- 0 = Admin, 1 = Driver
    joined_at    DATETIME(6)  NOT NULL,
    CONSTRAINT p_k_company_memberships PRIMARY KEY (id)
) CHARACTER SET utf8mb4;

-- ----------------------------------------------------------------------------
-- PARTE 2 — BACKFILL DE DATOS (§9.2)
-- ----------------------------------------------------------------------------

-- 2.1) Generar código para compañías sin código
UPDATE companies
SET invitation_code = UPPER(SUBSTRING(REPLACE(UUID(), '-', ''), 1, 8))
WHERE invitation_code IS NULL OR invitation_code = '';

-- 2.2) Crear membresía Admin (member_role = 0) para el creador de cada compañía
INSERT INTO company_memberships (company_id, user_id, member_role, joined_at)
SELECT c.id, c.fk_id_user, 0, UTC_TIMESTAMP()
FROM companies c
WHERE c.fk_id_user IS NOT NULL
  AND NOT EXISTS (
    SELECT 1 FROM company_memberships m
    WHERE m.company_id = c.id AND m.user_id = c.fk_id_user
  );

-- ----------------------------------------------------------------------------
-- PARTE 3 — RESTRICCIONES FINALES (ejecutar tras el backfill)
-- ----------------------------------------------------------------------------

-- 3.1) invitation_code ya no admite NULL una vez backfilleado
ALTER TABLE companies
    MODIFY COLUMN invitation_code VARCHAR(12) NOT NULL;

-- 3.2) Índices únicos (igual que la migración EF)
CREATE UNIQUE INDEX i_x_companies_invitation_code
    ON companies (invitation_code);

CREATE UNIQUE INDEX i_x_company_memberships_user_id
    ON company_memberships (user_id);

CREATE UNIQUE INDEX i_x_company_memberships_company_id_user_id
    ON company_memberships (company_id, user_id);

-- ============================================================================
--  FIN. Verifica:
--    SELECT id, name, fk_id_user, invitation_code FROM companies;
--    SELECT * FROM company_memberships;
-- ============================================================================
