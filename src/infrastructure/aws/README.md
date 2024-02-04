
## Bootstrap

Prerequirements:
- existing aws account
- existing access key
- access keys stored in `~/.aws/credentials`
- _aws cli_ installed
- powershell core installed


### create terraform state in aws s3
Only once
```powershell
$env:AWS_PROFILE = 'YOUR AWS PROFILE IN ~/.aws/credentials'
./bootstrap.ps1
```

### create and update terraform ressources within aws
```powershell
cd envs/dev
$env:AWS_PROFILE='YOUR AWS PROFILE IN ~/.aws/credentials'
tf init
tf apply
```