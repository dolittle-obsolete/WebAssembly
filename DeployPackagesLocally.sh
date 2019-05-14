#!/bin/bash
export PACKAGEDIR=$PWD/Packages
export PACKAGEVERSION=3.1000.0
export TARGETROOT=~/.nuget/packages

if [ ! -d "$PACKAGEDIR" ]; then
    mkdir $PACKAGEDIR
fi

rm $PACKAGEDIR/*
dotnet pack ./Source/Core -p:PackageVersion=$PACKAGEVERSION --include-symbols --include-source -o $PACKAGEDIR
find $TARGETROOT/Dolittle.Interaction.WebAssembly.Core -name $PACKAGEVERSION -exec rm -rf {} \;
target=$TARGETROOT/Dolittle.Interaction.WebAssembly.Core/$PACKAGEVERSION
mkdir -pv $target && cp -v $PACKAGEDIR/Dolittle.Interaction.WebAssembly.Core.3.1000.0.nupkg $target
# Unzip package
tar -xzf $PACKAGEDIR/Dolittle.Interaction.WebAssembly.Core.3.1000.0.nupkg -C $target
# Create an empty .sha512 file, or else it won't recognize the package for some reason
touch $target/Dolittle.Interaction.WebAssembly.Core.3.1000.0.nupkg.sha512

dotnet pack -p:PackageVersion=$PACKAGEVERSION --include-symbols --include-source -o $PACKAGEDIR

for f in $PACKAGEDIR/*.symbols.nupkg; do
  mv ${f} ${f/.symbols/}
done

for f in $PACKAGEDIR/*.nupkg; do
    echo ""
    packagename=$(basename ${f%.3.1000.0.nupkg})
    target=$TARGETROOT/$packagename/$PACKAGEVERSION
    # Delete outdated .nupkg 
    find $TARGETROOT/$packagename -name $PACKAGEVERSION -exec rm -rf {} \;

    mkdir -pv $target && cp -v $f $target
    # Unzip package
    tar -xzf $target/$(basename $f) -C $target
    # Create an empty .sha512 file, or else it won't recognize the package for some reason
    touch $target/$(basename $f).sha512
done