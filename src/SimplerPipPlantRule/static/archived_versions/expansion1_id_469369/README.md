# SimplerPipPlantRule

## EN Simpler Pip Plant Rule

I really agree that the pips' vanilla plant rule is not hard but pure annoying.

So I made this, tweaked the plantDetectionRadius and maxPlantsInRadius arguments, changed the CountNearbyPlants function, to simplify the pips' plant rule.

There's a config file so that you can tweak it as you wish.

For default, the pip won't plant seed at the tile, if there's already a plant in the 3*3 square area of it, just like the preview picture.

[h1]Details[/h1]

[b]Now having config UI implement with PLib, check it out while mod's enabled. It's best to restart the game after you changed the config.[/b]

[b]Config Params (the config.json file in mod directory)[/b]
- searchMinInterval = 60f -> min interval for seed search
- searchMaxInterval = 300f -> max interval for seed search
- plantDetectionRadius = 1 (vanilla 6) -> detection area is a square area with 'plantDetectionRadius * 2 + 1' length of the sides and centered at the target tile.
- maxPlantsInRadius = 0 (vanilla 2) -> max allowed plants in detection area of the target tile is 'maxPlantsInRadius + 1'

[b]Patterns (Not strictly tested)[/b]
- 0101010 (Default as the preview): plantDetectionRadius = 1, maxPlantsInRadius = 0
- 0011001100: plantDetectionRadius = 2, maxPlantsInRadius = 1

[h1]Known Glitch[/h1]

- Still tweaking for balance, welcome for your ideas.

[h1]Useful Links[/h1]

- [url=https://github.com/as-limc/oni-mods]My GitHub[/url]
- [url=https://forums.kleientertainment.com/forums/forum/204-oxygen-not-included-mods-and-tools/][Oxygen Not Included] - Mods and Tools - Klei Entertainment Forums[/url]
- [url=https://oni-db.com/]Oxygen Not Included Database by Fabrizio Filizola[/url]
- [url=https://github.com/peterhaneve/ONIMods/tree/master/PLib]peterhaneve/ONIMods/PLib[/url]

## CN 简化树鼠种植规则

目前树鼠的种植规则确实不是难，就是单纯很烦人。

所以我调了下plantDetectionRadius和maxPlantsInRadius这两个参数，改了下CountNearbyPlants函数的逻辑，以便简化树鼠种植规则。

提供了配置文件可供按自己喜好调整参数。

默认情况下，如果在某个格子的3*3矩形范围内已经有一棵植物了，树鼠就不会在该格子种植，即如预览图所示。

[h1]具体参数[/h1]

[b]新增配置界面，用PLib实现，MOD开启时即可看见配置按钮，修改配置后最好是重启游戏以使配置生效。[/b]

[b]配置参数（MOD目录下的config.json文件中）[/b]
- searchMinInterval = 60f -> 种子搜寻最小间隔
- searchMaxInterval = 300f -> 种子搜寻最大间隔
- plantDetectionRadius = 1 (原生6) -> 植物探测半径，探测范围即以指定格子为中心边长为'plantDetectionRadius * 2 + 1'的正方形
- maxPlantsInRadius = 0 (原生2) -> 探测范围内最多允许已有的植物，超过这个值当前格子就不能种植，也即指定格子探测范围内允许种的植物数量最大为'maxPlantsInRadius + 1'

[b]模式 (没有严格测试)[/b]
- 0101010 (默认如预览图): plantDetectionRadius = 1, maxPlantsInRadius = 0
- 0011001100: plantDetectionRadius = 2, maxPlantsInRadius = 1

[h1]已知缺陷[/h1]

- 仍在微调平衡性，欢迎建议。

[h1]相关链接[/h1]

- [url=https://github.com/as-limc/oni-mods]My GitHub[/url]
- [url=https://forums.kleientertainment.com/forums/forum/204-oxygen-not-included-mods-and-tools/][Oxygen Not Included] - Mods and Tools - Klei Entertainment Forums[/url]
- [url=https://oni-db.com/]Oxygen Not Included Database by Fabrizio Filizola[/url]
- [url=https://github.com/peterhaneve/ONIMods/tree/master/PLib]peterhaneve/ONIMods/PLib[/url]
