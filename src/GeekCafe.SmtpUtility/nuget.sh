#!/bin/bash

nuget_api_key=""

echo "your key is: ${1}"

if [ -z "${1}" ]    
then

      echo "you need to supply your api key as the first parameter"
      exit -1
else
    
    nuget_api_key="${1}"

    echo "api key found."
    # todo auto increment
    # build it
    dotnet build --configuration Release

    # pack it
    dotnet pack --configuration Release

    # publish it
    # skip duplicates
    # todo get the version number automatically
    dotnet nuget push bin/Release/GeekCafe.SmtpUtility.1.0.0.nupkg  -k "${nuget_api_key}"  -s https://api.nuget.org/v3/index.json --skip-duplicate
fi



