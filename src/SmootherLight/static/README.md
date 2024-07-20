# SmootherLight

## EN Smoother Light

[strike]Rewrote the light radius algorithm[/strike], making the shape and brightness much more smoother than vanilla, can be configured to allow light go through MeshTile/AirflowTile

2024-07-20 21:45:20
Implement with new way, added a new setting to allow configuring light range tolerance (should affect all light buildings, use with causious）

[h1]Details[/h1]

[b]Now having config UI implement with PLib, check it out while mod's enabled. It's best to restart the game after you changed the config.[/b]

- LightThroughMeshTiles: true -> light can go though MeshTile/AirflowTile; false (default) -> can't
- LightRangeTolerance: 1 -> Tolerance when checking if a cell is in light range, the bigger the farther
    - Default to 1 which is slight bigger than vanilla but smaller then the old implementation
    - Set to 4 should be the same as the old implementation

[h1]Known Glitch[/h1]

- Still tweaking for balance, welcome for your ideas.

[h1]Useful Links[/h1]

- [url=https://github.com/viva-la-v/oni-mods]My GitHub[/url]
- [url=https://forums.kleientertainment.com/forums/forum/204-oxygen-not-included-mods-and-tools/][Oxygen Not Included] - Mods and Tools - Klei Entertainment Forums[/url]
- [url=https://oni-db.com/]Oxygen Not Included Database by Fabrizio Filizola[/url]
- [url=https://github.com/peterhaneve/ONIMods/tree/master/PLib]peterhaneve/ONIMods/PLib[/url]
## CN 光线更平滑
[strike]重写了光线范围算法[/strike]，使得光线形状和亮度相比原版更加平滑，可配置是否允许光线穿过网格砖/透气砖.

2024-07-20 21:45:20
刚测了下发现跳出，简化修改了实现方式，新增了一个参数用来自定义控制光照范围容差（对所有光照建筑物生效，谨慎调整使用）

[h1]具体参数[/h1]

[b]新增配置界面，用PLib实现，MOD开启时即可看见配置按钮，修改配置后最好是重启游戏以使配置生效。[/b]

- LightThroughMeshTiles: true -> 光线可以穿过网格砖/透气砖; false (默认值) -> 不能
- LightRangeTolerance: 1 -> 光照范围容差，数值越大、光照范围越大
    - 默认值为1比原版略大比之前的调整版略小
    - 设置为2则与旧版实现调整后的光照范围相等

[h1]已知缺陷[/h1]

- 仍在微调平衡性，欢迎建议。

[h1]相关链接[/h1]

- [url=https://github.com/viva-la-v/oni-mods]My GitHub[/url]
- [url=https://forums.kleientertainment.com/forums/forum/204-oxygen-not-included-mods-and-tools/][Oxygen Not Included] - Mods and Tools - Klei Entertainment Forums[/url]
- [url=https://oni-db.com/]Oxygen Not Included Database by Fabrizio Filizola[/url]
- [url=https://github.com/peterhaneve/ONIMods/tree/master/PLib]peterhaneve/ONIMods/PLib[/url]
