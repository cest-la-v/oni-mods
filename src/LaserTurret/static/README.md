# LaserTurret

## EN

Laser Turret

Tired of trying to suffocate those adorable little things, I made this Laser Turret, to target and attack nearby(in
current room and not blocked) critters, and can be controlled by signal.

ATTENTION: Reported that there might be unexpected killing that cleared the whole room, though I haven't encountered
yet. Still, the best and safest way of using this should be put it in a separate killing room.

[h1]Details[/h1]

* Materials: Refined Metal * 100
* Power: 120W
* Skill: Critter Ranching II
* Range: 7 tiles
* Size: 1W * 1H
* Tracking algorithm: 
    * critters in current room
    * critters selected in the filter
    * lowest reproduction profit, as on the same condition, how many times there are in the left living cycles
        * left_living_cycles = total_living_cycles - past_living_cycles
        * current_reproduction_cycles = cycles_per_reproduction * current_reproduction_rate
        * reproduction_profit = (left_living_cycles + current_reproduction_cycles) / cycles_per_reproduction
        * Anything without reproduction rate always has the highest reproduction profit
* Signal Logic: Continuously attack when not connected; Wait until target's completely dead after each kill when
  connected with True; Disabled when connected with False

[h1]Known Glitch[/h1]

* When critters get too close to the laser gun, there might be some anim glitch, but with no function influence.
* The critter detect algorithm mighty not work properly due to concurrent issue while using Spawner in Sandbox Mode.
  Reload or change the room capacity(add/remove block or etc.) will make it right again.

[h1]Useful Links[/h1]

* [url=https://github.com/cest-la-v/oni-mods]My GitHub[/url]
* [url=https://forums.kleientertainment.com/forums/forum/204-oxygen-not-included-mods-and-tools/][Oxygen Not Included] -
  Mods and Tools - Klei Entertainment Forums[/url]
* [url=https://oni-db.com/]Oxygen Not Included Database by Fabrizio Filizola[/url]

## CN

激光炮塔

厌倦了想方设法掐死这群可爱的小玩意儿，做了个激光炮塔，会自动瞄准并攻击周围的(同一个房间内未被挡住的)动物，可以用信号控制。

[h1]具体参数[/h1]

* 材料：100精炼金属
* 耗电：120瓦
* 技能：二级牧业
* 范围：7格
* 大小：1宽 * 1高
* 追踪算法：
    * 当前房间的小动物
    * 过滤器选中的小动物种类
    * 剩余繁殖价值最低，也即繁殖条件相同时，剩余年龄（单位：周期）还能繁殖多少次
        * 剩余存活周期 = 总生命周期 - 已存活周期
        * 当前繁殖已消耗周期 = 每次繁殖所需周期 * 当前繁殖度
        * 剩余繁殖次数 = (剩余存活周期 + 当前繁殖已消耗周期) / 每次繁殖所需周期
        * 任何无繁殖度的对象始终拥有最高繁殖价值
* 信号逻辑：信号未连接时持续攻击；信号连接为激活时会在每次击杀之后等待目标完全死亡；信号连接为禁用时会停止工作

[h1]已知缺陷[/h1]

* 距离太近的时候激光动画可能有点小毛病，不影响功能使用。
* 在沙盒模式用生成工具生成动物时，动物探测算法可能会出现由并发导致的探测不到某些动物的问题，SL或改变房间容量等操作可以刷新算法以修正问题。

[h1]相关链接[/h1]

* [url=https://github.com/cest-la-v/oni-mods]My GitHub[/url]
* [url=https://forums.kleientertainment.com/forums/forum/204-oxygen-not-included-mods-and-tools/][Oxygen Not Included] -
  Mods and Tools - Klei Entertainment Forums[/url]
* [url=https://oni-db.com/]Oxygen Not Included Database by Fabrizio Filizola[/url]