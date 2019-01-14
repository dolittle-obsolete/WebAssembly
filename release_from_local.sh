#!/bin/bash
# Assumes that the NuGet API key has been set: 
# $ nuget setApiKey <key> -Source https://api.nuget.org/v3/index.json
export TARGETROOT=~/.nuget/packages
export PACKAGEDIR=$PWD/Artifacts/NuGet
export DOLITTLERELEASE=true
export PACKAGEVERSION=$(git tag -l | sort -V --reverse | head -1)

package=$TARGETROOT/Dolittle.Interaction.WebAssembly.Core.$PACKAGEVERSION.nupkg
packagesha=$TARGETROOT/Dolittle.Interaction.WebAssembly.Core.$PACKAGEVERSION.nupkg.sha512

echo $package
echo $packagesha

rm -rf $PWD/Artifacts
$PWD/Build/build.sh UpdateVersionForProjects


dotnet pack ./Source/Core --include-symbols --include-source -o $PACKAGEDIR

find $TARGETROOT/Dolittle.Interaction.WebAssembly.Core -name $PACKAGEVERSION -exec rm -rf {} \;
target=$TARGETROOT/Dolittle.Interaction.WebAssembly.Core/$PACKAGEVERSION
mkdir -pv $target && cp -v $package $target
# Unzip package
tar -xzf $package -C $target
# Create an empty .sha512 file, or else it won't recognize the package for some reason
touch $packagesha


$PWD/Build/build.sh DeployFromLocal

for f in $PACKAGEDIR/*.symbols.nupkg; do
    nuget push $f -Source https://api.nuget.org/v3/index.json   
done

cd $PWD/Build
git reset --hard
cd $PWD