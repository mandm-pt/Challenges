string g="Green",b="Blue",d="Dark",l="Light",m="Medium",v="Violet",y="Yellow",s=$"|IndianRed|cd5c5c|{l}Coral|f08080|Salmon|fa8072|{d}Salmon|e9967a|{l}Salmon|ffa07a|Red|ff0000|Crimson|dc143c|FireBrick|b22222|{d}Red|8b0000|Pink|ffc0cb|{l}Pink|ffb6c1|HotPink|ff69b4|DeepPink|ff1493|{m+v}Red|c71585|Pale{v}Red|db7093|Coral|ff7f50|Tomato|ff6347|OrangeRed|ff4500|{d}Orange|ff8c00|Orange|ffa500|Gold|ffd700|{y}|ffff00|{l+y}|ffffe0|LemonChiffon|fffacd|{l}GoldenRod{y}|fafad2|PapayaWhip|ffefd5|Moccasin|ffe4b5|PeachPuff|ffdab9|PaleGoldenRod|eee8aa|Khaki|f0e68c|{d}Khaki|bdb76b|Lavender|e6e6fa|Thistle|d8bfd8|Plum|dda0dd|{v}|ee82ee|Orchid|da70d6|Fuchsia|ff00ff|Magenta|ff00ff|{m}Orchid|ba55d3|{m}Purple|9370db|{b+v}|8a2be2|{d+v}|9400d3|{d}Orchid|9932cc|{d}Magenta|8b008b|Purple|800080|Indigo|4b0082|{d}Slate{b}|483d8b|Slate{b}|6a5acd|{m}Slate{b}|7b68ee|RebeccaPurple|663399|{g+y}|adff2f|Chartreuse|7fff00|Lawn{g}|7cfc00|Lime|00ff00|Lime{g}|32cd32|Pale{g}|98fb98|{l+g}|90ee90|Spring{g}|00ff7f|{m}Spring{g}|00fa9a|{m}Sea{g}|3cb371|Sea{g}|2e8b57|Forest{g}|228b22|{g}|008000|{d+g}|006400|{y+g}|9acd32|OliveDrab|6b8e23|Olive|808000|{d}Olive{g}|556b2f|{m}Aquamarine|66cdaa|{d}Sea{g}|8fbc8f|{l}Sea{g}|20b2aa|{d}Cyan|008b8b|Teal|008080|Aqua|00ffff|Cyan|00ffff|{l}Cyan|e0ffff|PaleTurquoise|afeeee|Aquamarine|7fffd4|Turquoise|40e0d0|{m}Turquoise|48d1cc|{d}Turquoise|00ced1|Cadet{b}|5f9ea0|Steel{b}|4682b4|{l}Steel{b}|b0c4de|Powder{b}|b0e0e6|{l+b}|add8e6|Sky{b}|87ceeb|{l}Sky{b}|87cefa|DeepSky{b}|00bfff|Dodger{b}|1e90ff|Cornflower{b}|6495ed|Royal{b}|4169e1|{b}|0000ff|{m+b}|0000cd|{d+b}|00008b|Navy|000080|Midnight{b}|191970|Cornsilk|fff8dc|BlanchedAlmond|ffebcd|Bisque|ffe4c4|NavajoWhite|ffdead|Wheat|f5deb3|Burlywood|deb887|Tan|d2b48c|RosyBrown|bc8f8f|SandyBrown|f4a460|GoldenRod|daa520|{d}GoldenRod|b8860b|Peru|cd853f|Chocolate|d2691e|SaddleBrown|8b4513|Sienna|a0522d|Brown|a52a2a|Maroon|800000|White|ffffff|Snow|fffafa|Honeydew|f0fff0|MintCream|f5fffa|Azure|f0ffff|Alice{b}|f0f8ff|GhostWhite|f8f8ff|WhiteSmoke|f5f5f5|SeaShell|fff5ee|Beige|f5f5dc|OldLace|fdf5e6|FloralWhite|fffaf0|Ivory|fffff0|AntiqueWhite|faebd7|Linen|faf0e6|LavenderBlush|fff0f5|MistyRose|ffe4e1|Gainsboro|dcdcdc|{l}Gray|d3d3d3|{l}Grey|d3d3d3|Silver|c0c0c0|{d}Gray|a9a9a9|{d}Grey|a9a9a9|Gray|808080|Grey|808080|DimGray|696969|DimGrey|696969|{l}SlateGray|778899|{l}SlateGrey|778899|SlateGray|708090|SlateGrey|708090|{d}SlateGray|2f4f4f|{d}SlateGrey|2f4f4f|Black|000000";foreach(var i in args)System.Console.WriteLine("#"+s.Substring(s.IndexOf($"|{i}|")+i.Length+2,6));