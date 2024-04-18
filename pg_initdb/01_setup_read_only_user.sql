CREATE USER analytics_r_user WITH PASSWORD 'analytics_r_user_password';

GRANT CONNECT ON DATABASE analytics_database TO analytics_r_user;

GRANT USAGE ON SCHEMA public TO analytics_r_user;

GRANT SELECT ON ALL TABLES IN SCHEMA public TO analytics_r_user;

ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT SELECT ON TABLES TO analytics_r_user;
