module default {
  type User {
    required property user_id -> str {
      constraint exclusive;
    }
    property step -> int32 {
      default := 0
    }
  }
}
