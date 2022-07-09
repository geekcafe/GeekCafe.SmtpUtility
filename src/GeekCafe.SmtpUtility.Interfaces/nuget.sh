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
   
    # fix parsing issues
    # get rid of some of the crud
    package_id=$(tr -dc '[[:print:]]' <<< "${package_id}")
    version=$(tr -dc '[[:print:]]' <<< "${version}")
    

    package="${package_id}.${version}.nupkg"
    echo "pacakge = ${package}"
   
    # build it
    dotnet build --configuration Release

    # pack it
    dotnet pack --configuration Release


    # publish it
    # skip duplicates    
    dotnet nuget push bin/Release/${package}  \
        -k "${nuget_api_key}"  \
        -s https://api.nuget.org/v3/index.json \
        --skip-duplicate
fi
