$region     = 'eu-central-1'
$bucketName = 'devops-terraform-b31a89ebc35b'

aws s3api `
    create-bucket `
    --bucket $bucketName `
    --region $region `
    --create-bucket-configuration LocationConstraint=$region

aws s3api `
    put-bucket-versioning `
    --bucket $bucketName `
    --versioning-configuration Status=Enabled

