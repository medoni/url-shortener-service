data "aws_ecr_authorization_token" "token" {}

# data "aws_iam_policy_document" "s3_ecr_access" {
#   version = "2012-10-17"
#   statement {
#     sid     = "s3access"
#     effect  = "Allow"
#     actions = ["*"]

#     principals {
#       type        = "*"
#       identifiers = ["ecs-tasks.amazonaws.com"]
#     }
#   }
# }