# Dokumentace projektu

Výstupem projektu je demo hry v raycastingovém 2.5D enginu, společně s Level Editorem.

**Obsah:**
  * [Raycaster](#raycaster)
    + [Návod k použití](#návod-k-použití)
      - [Před spuštěním](#před-spuštěním)
    + [Jak ray casting funguje?](#jak-ray-casting-funguje)
    + [Použité technologie](#použité-technologie)
  * [Level Editor](#level-editor)
    + [Návod k použití](#návod-k-použití-1)
      - [Před spuštěním](#před-spuštěním-1)
      - [Přehled rozhraní](#přehled-rozhraní)
      - [Načítání/ukládání souborů](#načítáníukládání-souborů)
      - [Práce s editorem](#práce-s-editorem)
    + [Formát ukládaných souborů](#formát-ukládaných-souborů)
    + [Použité technologie](#použité-technologie-1)
  * [Formát souborů](#formát-souborů)

## Raycaster

### Návod k použití

#### Před spuštěním

Raycaster musí mít přístup k potřebným texturám ve složce **textures** a levelům ve složce **levels**. Složky musí být na stejném místě jako Raycaster.

Potřebné textury jsou: 
- **textureMap.txt** s hodnotami: 
	- **count** - počet textur v mapě
	- **width**, **height** - rozměry textur 
	- **textureMap** - RGB hodnoty všech pixelů
- **skyBox.txt** s hodnotami:
	- **width**, **height** - rozměry textury
	- **skyBox** - RGB hodnoty všech pixelů

Potřebný je minimálně level **map_1.txt**, poté případně další podle hodnoty **nextLevel** v souboru. 
- Levely mají hodnoty:
	- **width**, **height** - rozměry levelu
	- **spawnX**, **spawnY** - pozice spawnpointu levelu
	- **nextLevel** - cesta k dalšímu levelu
	- **mapWalls**, **mapFloors**, **mapCeilings** - mapy textur

Soubory viz [Formát souboru](#formát-souborů)

### Jak ray casting funguje?

Ray casting je způsob renderování využívaný v jedněch z prvních 3D her z pohledu první osoby, jako například **Wolfenstein 3D**, který žánr zpopularizoval, nebo později **Doom**, který používal už komplikovanější engine.

Raycasting je velmi výpočetně jednoduchý, protože, i přes to, že výsledný pohled vypadá trojrozměrně, používá 2D mapu, proto se hráč například nemůže podívat nad a pod sebe, a nemůže se pohybovat v třetí dimenzi.

Hráč při raycastingu "vyšle" paprsky (odtud název) do všech směrů, kam s aktuálním natočením vydí a podle toho jak daleko od něj paprsky narazí do zdi, nakreslí čáru na obrazovce (paprsek narazí dál - menší čára, blíž - větší čára) a tím vytvoří iluzi 3D pohledu.

Raycasting z Wikipedie:

![](https://upload.wikimedia.org/wikipedia/commons/e/e7/Simple_raycasting_with_fisheye_correction.gif)

Tímto způsobem jdou zobrazit pouze zdi, pro podlahu a strop jsou dvě vlastní 2D mapy. Pro jejich renderování se musí přepočítat pozice na obrazovce na pozici na mapě, což je ale výpoočetně náročnější, a tak starší raycastingové enginy používají pro podlahu a strop často jenom jednoduchou barvu.

### Použité technologie

Raycaster je napsán v jazyce **C** a využívá grafickou knihovnu **GLFW** pro **OpenGL**, která umožňuje spravovat okna a ovládat vstup z myši a klávesnice.

## Level Editor


### Návod k použití

#### Před spuštěním

Level Editor musí mít přístup ke stejné mapě textur jako samotný Raycaster. Musí být proto umístěn ve stejné složce jako Raycaster, která obsahuje podsložku **textures** se souborem **textureMap.txt**.

#### Přehled rozhraní

#### Načítání/ukládání souborů

Cesta k aktuálně upravovanému souboru je v levé horní části okna, pokud ještě není určena je místo cesty k souboru napsáno "Nový soubor". Pokud byla provedena práce, která zatím nebyla uložena, zobrazí se vedle cesty k souboru hvězdička *.

Načítání a ukládání se provádí tlačítkami v pravém horním rohu, pokud není vybraná cesta k souboru, zobrazí se okno výběru souboru, pro načítání je nutné vybrat již existující soubor, pro uložení nikoliv. Pokud chcete uložit soubor na jiné místo než původní, použijte tlačítlo "Uložit jako...".

Raycaster používá levely ve složce **levels**, po spuštění hry bude automaticky načtena mapa **map_1.txt**.

#### Práce s editorem

Pro změnu textury se musí vybraná textura stisknout na paletě textur na vršku obrazovky, vybraná textura je zvýrazněna odlišným rámečkem.

Uprostřed editoru jsou tři tabulky, mapy, jedna pro zdi levelu, jedna pro podlahu a jedna pro strop. Na každé z map jdou upravovat textury levým tlačítkem myši, při stisknutí bude na vybranou pozici umístěna vybraná textura z palety. Při stisknutí pravého tlačítka je automaticky umístěna prázdná textura.

Velikost map se dá změnit stisknutím tlačítek **+**, nebo **-** vedle první mapy. Pokud mapa nejde dál zmenšit tlačítko **-** se skryje.

Nad paletou textur je žlutý čtverec, který se dá vybrat stejně jako textura. Když je čtvereček vybrán, klinutí na mapu způsobí posunutí spawnpointu levelu, znázorněného žlutým rámečkem kolem textury.

Vedle žlutého čtverce je textové pole pro zadání cesty k dalšímu levelu.

### Formát ukládaných souborů

Při uložení jsou v souboru vybrány takové prázdné místa, nové řádky, aby byl soubor lehce čitelný. Každá hodnota je na vlastním řádku, mezi nesouvisejícimi hodnotami je mezara a v mapách je každý řádek oddělen. Například:
```
# width 8
# height 8

# spawnX 2
# spawnY 2

# nextLevel ../levels/map_2.txt

# mapWalls
0 0 0 0 0 0 0 0 
0 0 0 0 0 0 0 0 
0 0 0 0 0 0 0 0 
0 0 0 2 2 0 0 0 
0 0 0 2 2 0 0 0 
0 0 0 0 0 0 0 0 
0 0 0 0 0 0 0 0 
0 0 0 0 0 0 0 0 

# mapFloors
1 1 1 1 1 1 1 1 
1 1 1 1 1 1 1 1 
1 1 1 1 1 1 1 1 
1 1 1 1 1 1 1 1 
1 1 1 1 1 1 1 1 
1 1 1 1 1 1 1 1 
1 1 1 1 1 1 1 1 
1 1 1 1 1 1 1 1 

# mapCeilings
1 1 1 1 1 1 1 1 
1 1 1 1 1 1 1 1 
1 1 1 1 1 1 1 1 
1 1 1 1 1 1 1 1 
1 1 1 1 1 1 1 1 
1 1 1 1 1 1 1 1 
1 1 1 1 1 1 1 1 
1 1 1 1 1 1 1 1 
```

### Použité technologie

Level editor je napsán v jazyze **C#** a pro grafické rozhraní využívá knihovnu **Windows Forms**.

## Formát souborů

Pro ukládání dat levelů a textur byl vytvořen jodnotný formát souborů schopný ukládat hodnoty různých typů v jednoduché a lidsky čitelné formě.

Příklad souboru - jednoduchý testovací level:
```
# width 8
# height 8

# spawnX 2
# spawnY 2

# nextLevel ../levels/map_2.txt

# mapWalls
0 0 0 0 0 0 0 0 
0 0 0 0 0 0 0 0 
0 0 0 0 0 0 0 0 
0 0 0 2 2 0 0 0 
0 0 0 2 2 0 0 0 
0 0 0 0 0 0 0 0 
0 0 0 0 0 0 0 0 
0 0 0 0 0 0 0 0 

# mapFloors
1 1 1 1 1 1 1 1 
1 1 1 1 1 1 1 1 
1 1 1 1 1 1 1 1 
1 1 1 1 1 1 1 1 
1 1 1 1 1 1 1 1 
1 1 1 1 1 1 1 1 
1 1 1 1 1 1 1 1 
1 1 1 1 1 1 1 1 

# mapCeilings
1 1 1 1 1 1 1 1 
1 1 1 1 1 1 1 1 
1 1 1 1 1 1 1 1 
1 1 1 1 1 1 1 1 
1 1 1 1 1 1 1 1 
1 1 1 1 1 1 1 1 
1 1 1 1 1 1 1 1 
1 1 1 1 1 1 1 1 
```

V souborech se vše odděluje prázdným místem, například mezerou, novou řádkou, nebo tabulátorem. Na druhu prázdného místa nezáleží, a proto se může vybrat nejvhodnější pro lehkou čitelnost souboru člověkem, pro příklad dva zápisy identické hodnoty:
```
# 

value 1 2 3 2 4 6 3 6 9
```
```
# value
1 2 3
2 4 6
3 6 9
```
Soubor se skládá z několika hodnot, hodnoty vždy začínají křížkem **#**, poté následuje jednoslovné jméno hodnoty a hodnota samotná. Hodnota může mít tři typy:

**celé číslo**
```
# value 42
```
**slovo**
```
# value helloWorld!
```
**pole celých čísel**
```
# value 1 1 2 3 5 8 13 21 34
```
Pole je vždy pouze jednorozměrné, pro více rozměrné pole o předem neznámé velikosti je potřeba zvlášť uvést rozměry pole jako vlastní hodnoty.
```
# width 3
# height 3

# 2DArray
1 2 3
2 4 6
3 6 9
```

Mezi jednotlivými hodnotami se může nacházet jakýkoliv text, komentáře, určené pro zjednodušení orientace v souboru.
