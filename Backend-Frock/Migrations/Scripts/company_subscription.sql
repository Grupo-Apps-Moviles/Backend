-- ============================================================================
--  Fase 3 — Subscription a nivel Compañía: SCHEMA DELTA para MySQL (Railway)
-- ============================================================================
--  IMPORTANTE
--  La base de datos de Railway se creó con `Database.EnsureCreated()` y NUNCA
--  ha usado migraciones EF (ver Migrations/Scripts/backfill_company_memberships.sql).
--  En un entorno NUEVO el esquema se crea solo desde el modelo (snake_case), por
--  lo que la tabla `subscriptions` ya nacería con `company_id` y `max_members`.
--
--  Para la BD de prueba YA EXISTENTE (que tiene `driver_id`), aplica este delta.
--  La BD es de prueba: no hay datos reales que migrar, así que NO hay backfill.
--  El owner del script soy yo (lo corro en local); este archivo NO se ejecuta
--  automáticamente.
--
--  Nombres de columnas en snake_case (UseSnakeCaseNamingConvention).
-- ============================================================================

-- 1) Renombrar driver_id -> company_id (misma semántica de FK lógica, INT NOT NULL).
ALTER TABLE subscriptions
    CHANGE COLUMN driver_id company_id INT NOT NULL;

-- 2) Nueva columna max_members (tope de miembros que habilita la suscripción).
--    DEFAULT 1 = tope free; al activar/renovar la app lo sube a 10, al cancelar
--    vuelve a 1 (lógica en el agregado Subscription).
ALTER TABLE subscriptions
    ADD COLUMN max_members INT NOT NULL DEFAULT 1;

-- ============================================================================
--  FIN. Verifica:
--    DESCRIBE subscriptions;
--    SELECT id, company_id, status, max_members FROM subscriptions;
-- ============================================================================
