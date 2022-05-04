#/bin/env

# link: https://play.picoctf.org/practice/challenge/256?category=3&originalEvent=70&page=1&search=

wget -q https://artifacts.picoctf.net/c/428/bloat.flag.py
wget -q https://artifacts.picoctf.net/c/428/flag.txt.enc

a="!\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_\`abcdefghijklmnopqrstuvwxyz{|}~ "

grep -Eo 'a\[\d+\]' < ./bloat.flag.py | while read -r line; do
    idx=$(tr -d "a[]"<<<$line)

    sed -i '' "s/a[[]$idx]/'${a:$idx:1}'/g"  bloat.flag.py
done

sed -i '' "s/'+'//g" bloat.flag.py

echo "happychance" | python3 bloat.flag.py | grep -Eo "picoCTF{\w+}"

rm ./bloat.flag.py
rm ./flag.txt.enc
