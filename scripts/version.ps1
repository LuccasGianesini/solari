[CmdletBinding()]
param (
  $BRANCH_NAME,
  $BUILD_ARTIFACT_DIR
)
# Fix current prod version pointing to beta.
# rev list not counting number of commits.


# On master use the patch currentVersion$currentVersion. if ther is a prod release with the same patch, increment patch by one.
# On releases/* tag is going to be '-bata.' + number of commits since last tag. If there is a patch bump. start with 1.
# On develop tag is going to be '-unstable.' + number of commits since last tag. If there is a patch bump. start with 1.

$global:targetMajor = '1' # Can be changed.
$global:targetMinor = '10' # Can be changed.
$global:targetPatch = $null #Patch is auto incremented.
$global:tags = (git tag --sort=refname)
$global:hasNoReleases = $null
$global:isCurrentVersionPreRelease = $null
$global:currentVersion = $null
$global:currentProdVersion = $null
$global:currentMajor = $null;
$global:currentMinor = $null;
$global:currentPatch = $null;
$global:branch = $null;

#REGEX
$global:preReleaseRegex = '\-[0-9a-zA-Z-]+\.[0-9]' 
$global:developmentBranchRegex = '^dev(elop)?(ment)?$' 
$global:releasesBranchRegex = '^releases?[/-]'
$global:isCurrentBranchRegex = '^\* (.*)'

$global:productionVersionRegex = '(([0-9]+)\.([0-9]+)\.([0-9]+)?)'
$global:versionExtractionRegex = '(?:0|[1-9]\d*)\.(?:0|[1-9]\d*)\.(?:0|[1-9]\d*)(?:-(?:0|[1-9]\d*|[\da-z-]*[a-z-][\da-z-]*)(?:\.(?:0|[1-9]\d*|[\da-z-]*[a-z-][\da-z-]*))*)?(?:\+[\da-z-]+(?:\.[\da-z-]+)*)?'

# TEST no branch input
# TEST with develop
# TEST with master
# TEST with releases/


function isCurrentBranch($branch) {
  return ([regex]::Match($branch , $global:isCurrentBranchRegex)).Success  
}
function extractBranchName($branch) {
  return $branch.substring(1).trim()
}
function setBranch {
  if ($null -eq $BRANCH_NAME -or $BRANCH_NAME -like '') {
    $currentBranch = ''
    git branch | ForEach-Object {
      if (isCurrentBranch $_) {
        $currentBranch += extractBranchName $_
      }
    }
    return $currentBranch
  }
  return $BRANCH_NAME
}
function hasNoReleases {
  if (($global:currentVersion -eq '0.0.0')) {
    return $true
  }
  return $false
}
function isPreRelease($version) {
  $match = [regex]::Match($version, $global:preReleaseRegex)
  return $match.Success
}
function getCommitsSinceLastTag {
  try {
    $commits = ([regex]::match((git describe --tags), '-([0-9])-').Groups[1].Value)
    if($commits -eq '')
    {
      Write-Host('Found 0 commits since last tag.')
    }else{
      Write-Host('Found ' + $commits + ' commits since last tag.')
    }

    if($commits -eq '')
    {
      return '0'
    }

    return $commits
  }
  catch {
    return '0'
  }

}
# Extracts the semver part of the string x.x.x-xxx.xxx
# Removes the v
function extractVersionFromTag ($version) {
  $match = [regex]::Match($version, $global:versionExtractionRegex)
  if ($match.Success) {
    return $match.Value
  }
  throw 'Unable to extract version from tag. Match result: ' + $match.Value;
}

function setCurrentVersion($versions) {
  if ($null -eq $versions) {
    return '0.0.0';
  }

  if ($versions -isnot [System.Array]) {
    return extractVersionFromTag $versions.ToString()
  }
  return extractVersionFromTag $versions[$versions.length - 1]
  
}
function getProdVersion($version) {
  $tag = extractVersionFromTag $version
  if (isPreRelease $tag -eq $false) {
    return $tag
  }
  return '0.0.0'
}
# function setCurrentProdVersion($versions) {
#   if ($null -eq $versions) {
#     return '0.0.0';
#   }
#   if ($versions -isnot [System.Array]) {
#     return getProdVersion $versions
#   }
#   $version = ''
#   for ($i = $versions.length; $i -gt 0 ; $i--) {
#     $version = getProdVersion $versions[$i - 1]
#   }
#   return $version
# }

# Extracts the patch value from the currentVersion
function getPatchFromCurrentVersion($currentVersion) {
  $splt = $currentVersion.split('.');
  return $splt[2]  
}
# Extracts the minor value from the currentVersion
function getMinorFromCurrentVersion($currentVersion) {
  $splt = $currentVersion.split('.');
  return $splt[1]  
}
# Extracts the major value from the currentVersion
function getMajorFromCurrentVersion($currentVersion) {
  $splt = $currentVersion.Split('.');
  return $splt[0]  
}

# Check if targetMajorVersion is different from currentMejorVersion
function hasMajorVersionChanged {
  if ($global:currentMajor -notlike $global:targetMajor) {
    return $true
  }
  return $false;
}
# Check if targetMinorVersion is different from currentMinorVersion
function hasMinorVersionChanged {
  if ($global:currentMinor -notlike $global:targetMinor) {
    return $true
  }
  return $false;
}

# Returns the current patch version.
function getCurrentPatch($version, $isPreRelease) {
  if ($isPreRelease) {
    return getPatchFromCurrentVersion $version.Split('-')[0] # TODO Use regex to extract version from pre-release
  }
  else {
    return getPatchFromCurrentVersion $version
  }
   
}
$global:branch = setBranch

Write-Output('Building from branch ' + $global:branch)
Write-Output('..................................................')

Write-Output('Analizing the current state')
Write-Output('..................................................')
Write-Output('')

$global:currentVersion = setCurrentVersion $global:tags
$global:isCurrentVersionPreRelease = isPreRelease $global:currentVersion
Write-Output('Current release is: ' + $global:currentVersion + '. Is current version pre-release? ' + $global:isCurrentVersionPreRelease)

#$global:currentProdVersion = setCurrentProdVersion $global:tags
Write-Output('Current production release is: ' + $global:currentProdVersion)

$global:hasNoReleases = hasNoReleases
$global:currentMajor = getMajorFromCurrentVersion $global:currentVersion
$global:currentMinor = getMinorFromCurrentVersion $global:currentVersion
$global:currentPatch = getCurrentPatch $global:currentVersion $global:isCurrentVersionPreRelease
Write-Output('Current major currentVersion is: ' + $global:currentMajor)
Write-Output('Current minor currentVersion is: ' + $global:currentMinor)
Write-Output('Current patch currentVersion is: ' + $global:currentPatch)

Write-Output('')
Write-Output('..................................................')

Write-Output('Building target versions based on current state')
Write-Output('')

function buildTargetMinorVersion {
  if (($global:targetMinor -as [int]) -lt ($global:currentMinor -as [int])) {
    throw 'The target minor version is lower then the current minor version.'
  }

  if (hasMajorVersionChanged) {
    Write-Host('There was a change in the major version. Minor will be set to 0')
    return '0'
  }
  return $global:targetMinor
}

function buildTargetMajorVersion {
  if (($global:targetMajor -as [int]) -lt ($global:currentMajor -as [int])) {
    throw 'The target major version is lower then the current major version.'
  }
  return $global:targetMajor
}


# If current version is pre and is on master - use current patch
# If current version is prod and is on dev/res - increase patch by 1

function buildTargetPatchVersion {
  # returns 1 so that the version doesn't ends being 0.0.0
  if ($global:hasNoReleases) {
    return '1'
  }
  # changes on either one resets the patch to 0
  if (hasMajorVersionChanged -or hasMinorVersionChanged) {
    Write-Host('There was a chenge in the major or minor versions. Patch version will be set to 0')
    return '0';
  }
  
  if($global:branch -like 'master')
  {
    Write-Host('Using master branch')
    if(isPreRelease $global:currentVersion)
    {
      Write-Host('Current version is pre-relese. Using current patch version')
      return $global:currentPatch
    }
    else
    {
      Write-Host('Current version is production release. Current patch will be incremented by 1')
      return ($global:currentPatch -as [int]) + 1
    }
  }
  
  Write-Host('Using pre-release branches')
  if(isPreRelease $global:currentVersion){
    return $global:currentPatch  
  }
  return ($global:currentPatch -as [int]) + 1
  
}


$global:targetPatch = buildTargetPatchVersion
$global:targetMinor = buildTargetMinorVersion
$global:targetMajor = buildTargetMajorVersion
Write-Output('..................................................')
Write-Output('Target major version is: ' + $global:targetMajor)
Write-Output('Target minor version is: ' + $global:targetMinor)
Write-Output('Target patch version is: ' + $global:targetPatch)
Write-Output('..................................................')
function buildDefaultVersionScheme {
  if ($global:targetMajor -as [int] -lt 0) {
    throw 'Major version cannot be a negative value'
  }
  if ($global:targetMinor -as [int] -lt 0) {
    throw 'Minor version cannot be a negative value'
  }
  if ($global:targetMajor -as [int] -lt 0) {
    throw 'Patch version cannot be a negative value'
  }
  return $global:targetMajor + '.' + $global:targetMinor + '.' + $global:targetPatch  
}
function buildBetaReleaseVersion {
  Write-Host('A beta version will be built')
  $scheme = buildDefaultVersionScheme
  if ($global:hasNoReleases) {
    return $scheme + '-beta.0'
  }
  if (hasMajorVersionChanged -or hasMinorVersionChanged) {
    return $scheme + '-beta.0'
  }
  $commits = getCommitsSinceLastTag
  return $scheme + '-beta.' + $commits
}
function buildUnstableReleaseVersion {
  Write-Host('An unstable version will be built')
  $scheme = buildDefaultVersionScheme
  if ($global:hasNoReleases) {
    return $scheme + '-unstable.0'
  }
  if (hasMajorVersionChanged -or hasMinorVersionChanged) {
    return $scheme + '-unstable.0'
  }
  $commits = getCommitsSinceLastTag
  return $scheme + '-unstable.' + $commits
}

function buildVersion {
  if ($global:branch -eq 'master') {
    return buildDefaultVersionScheme
  }
  if ([regex]::Match($global:branch, $global:releasesBranchRegex).Success) {
    return buildBetaReleaseVersion

  }
  if ([regex]::Match($global:branch, $global:developmentBranchRegex).Success) {
    return buildUnstableReleaseVersion
  }
}

#Debugging function only
#function tag($ver) {
#  git tag -a ('v' + $ver) -m 'Automatically tagged by ps1 script'
#}
$version = buildVersion
Write-Output('..................................................')
Write-Output('New version is: ' + $version);
Write-Output('..................................................')
Write-Output('Setting envrionment variable MY_BUILD_NUMBER')
Write-Host "##vso[task.setvariable variable=my_build_number;]$version"