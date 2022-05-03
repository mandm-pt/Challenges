#/bin/env

# link: https://play.picoctf.org/practice/challenge/266?bookmarked=0&category=3&originalEvent=70&page=1&solved=0

wget -q https://artifacts.picoctf.net/c/308/run

strings ./run | grep -Eo "picoCTF{\w+}"

rm ./run