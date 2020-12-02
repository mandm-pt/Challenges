# Level 11 to Level 12

Check the source code:
* It uses XOR
* Stores a cookie named "data"
* It will show the password if the cookie has the property "showpassword" equals to "yes"

1. The first time the page is loaded, it uses `defaultdata` which is a json object:
   1. Go to an online php code website and test the following code
   ```
   $defaultdata = array( "showpassword"=>"no", "bgcolor"=>"#ffffff");
   echo json_encode($defaultdata);
   ```
   That will output the following: `{"showpassword":"no","bgcolor":"#ffffff"}`
2. Check [XOR Cipher](https://en.wikipedia.org/wiki/XOR_cipher), can be translated in:
   1. json XOR key = secret
   2. `{"showpassword":"no","bgcolor":"#ffffff"}` XOR key = `ClVLIh4ASCsCBE8lAxMacFMZV2hdVVotEhhUJQNVAmhSEV4sFxFeaAw%3D`
   3. Using the XOR properties we can do:
        json XOR secret = key
3. According to the source code `$tempdata = json_decode(xor_encrypt(base64_decode($_COOKIE["data"])), true);` we need to do the following steps in order to decrypt:
   1. base64 decode the contents of the cookie
   2. Perform the XOR operation
   3. json decode (this is just to convert the text into an object. Can be ignored)

Using [CyberChef](https://gchq.github.io/CyberChef/#recipe=From_Base64('A-Za-z0-9%2B/%3D',true)XOR(%7B'option':'UTF8','string':'%7B%22showpassword%22:%22no%22,%22bgcolor%22:%22%23ffffff%22%7D'%7D,'Standard',false)&input=Q2xWTEloNEFTQ3NDQkU4bEF4TWFjRk1aVjJoZFZWb3RFaGhVSlFOVkFtaFNFVjRzRnhGZWFBdyUzRA)
**Output** `qw8Jqw8Jqw8Jqw8Jqw8Jqw8Jqw8Jqw8Jqw8Jqw8JqL.`

According to Wikipedia `By itself, using a constant repeating key, a simple XOR cipher can trivially be broken using frequency analysis`

So the key is `qw8J`

Now we can modify the contents of json and encrypt it with the key:
`{"showpassword":"yes","bgcolor":"#ffffff"}` XOR `qw8J` = secret

Using [CyberChef](https://gchq.github.io/CyberChef/#recipe=XOR(%7B'option':'UTF8','string':'qw8J'%7D,'Standard',false)To_Base64('A-Za-z0-9%2B/%3D')&input=eyJzaG93cGFzc3dvcmQiOiJ5ZXMiLCJiZ2NvbG9yIjoiI2ZmZmZmZiJ9)
**Output** `ClVLIh4ASCsCBE8lAxMacFMOXTlTWxooFhRXJh4FGnBTVF4sFxFeLFMK`

Setting the cookie with the above output and refresh the page we get: `The password for natas12 is EDXp0pS26wLKHZy1rDBPUZk0RKfLGIR3`