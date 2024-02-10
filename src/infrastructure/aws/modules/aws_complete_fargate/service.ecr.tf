

# resource "aws_ecr_repository" "ecr_repository" {
#   name = "${var.project-name-short}-${var.short-env-name}-ecr-repo"

#   image_scanning_configuration {
#     scan_on_push = true
#   }
# }

# resource "aws_ecr_lifecycle_policy" "ecr_lifecycle_policy" {
#   repository = aws_ecr_repository.ecr_repository.name

#   policy = <<EOF
# {
#   "rules": [
#     {
#       "rulePriority": 1,
#       "description": "Expire images older than 14 days",
#       "selection": {
#         "tagStatus": "any",
#         "countType": "sinceImagePushed",
#         "countUnit": "days",
#         "countNumber": 14
#       },
#       "action": {
#         "type": "expire"
#       }
#     }
#   ]
# }
# EOF
# }