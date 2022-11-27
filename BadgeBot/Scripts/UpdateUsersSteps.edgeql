insert User {
  user_id := <str>$user_id,
  step := <int32>$step
}
unless conflict on .user_id
else (
  update User 
  set {
    step := <int32>$step if .step < <int32>$step else .step
  }
)