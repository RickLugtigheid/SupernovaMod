---
layout: default
title: Flying Terror

npc:
    name: Flying Terror
    icon: /SupernovaMod/assets/images/FlyingTerror_icon.gif
    map_icon: https://raw.githubusercontent.com/KoekMeneer/SupernovaMod/main/Content/Npcs/Bosses/FlyingTerror/FlyingTerror_Head_Boss.png

    # Normal mode stats
    damage: 36
    max_life: 3400
    defense: 12
    knockback_resist: 100%
    environment:
        - <a href="https://terraria.wiki.gg/wiki/Layers#Surface">Surface</a> + <a href="https://terraria.wiki.gg/wiki/Night">Night</a>
    # drops:
    #     - name: myItem
    #       url: a
    #       icon: a
    #       quantity: 1
    #       drop_rate: 100%
    #     - id: verglas_bar
    #       quantity: 1-10
    #       drop_rate: 50%

parent: Bosses
---

# Flying Terror
---
{% include components/stats-npc.html %}