#/bin/env

# link: https://play.picoctf.org/practice/challenge/267?bookmarked=0&category=3&originalEvent=70&page=1&solved=0

wget -q https://artifacts.picoctf.net/c/351/run

strings ./run | grep -Eo "picoCTF{\w+}"

rm ./run