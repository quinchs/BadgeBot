CREATE MIGRATION m1jmzsubk43b3qxu45vzs3yxzrub325rn4zq3u3grwsvexe7ae7i2q
    ONTO initial
{
  CREATE FUTURE nonrecursive_access_policies;
  CREATE TYPE default::User {
      CREATE PROPERTY step -> std::int32 {
          SET default := 0;
      };
      CREATE REQUIRED PROPERTY user_id -> std::str {
          CREATE CONSTRAINT std::exclusive;
      };
  };
};
