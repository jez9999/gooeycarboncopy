echo Deleting bin directories:
find . -name "bin" -type d -exec rm -rvf '{}' \;
echo 
echo Deleting obj directories:
find . -name "obj" -type d -exec rm -rvf '{}' \;

