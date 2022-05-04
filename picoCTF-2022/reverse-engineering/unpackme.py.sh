#/bin/env

# link: https://play.picoctf.org/practice/challenge/314?category=3&originalEvent=70&page=1&search=

wget -q https://artifacts.picoctf.net/c/464/unpackme.flag.py

cat unpackme.flag.py | head -11 >> patch.py
echo "print(plain.decode())" >> patch.py

python3 patch.py | grep -Eo "picoCTF{\w+}"

rm ./unpackme.flag.py
rm ./patch.py