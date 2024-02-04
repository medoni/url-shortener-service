resource "aws_ecr_repository" "acr-repository" {
  name = "uss-${var.short-env-name}-acr-repo"
}