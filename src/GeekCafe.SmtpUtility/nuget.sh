#!/bin/bash

if [ -z "${1}" ]    
then

      echo "you need to supply your api key as the first parameter"
      exit -1
else
    
    nuget_api_key="${1}"

    # the project file
    project_file=$(ls | grep *.csproj)     
    echo $project_file
       

    # just store it in memory
    version=$(grep '<Version>' < $project_file | sed 's/.*<Version>\(.*\)<\/Version>/\1/' | tee /dev/tty)    
    
    # get the package id
    package_id=$(grep '<PackageId>' < $project_file | sed 's/.*<PackageId>\(.*\)<\/PackageId>/\1/' | tee /dev/tty)    
    
    echo "api key found."    
    # build it
    dotnet build --configuration Release

    # pack it
    dotnet pack --configuration Release

    # publish it
    # skip duplicates    
    dotnet nuget push bin/Release/${package_id}.${version}.nupkg  \
        -k "${nuget_api_key}"  \
        -s https://api.nuget.org/v3/index.json \
        --skip-duplicate
fi



