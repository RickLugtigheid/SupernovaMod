---
layout: default
title: Starry Night
nav_order: 2
item:
    name: Starry Night
    icon: https://raw.githubusercontent.com/KoekMeneer/SupernovaMod/main/Content/Items/Weapons/Ranged/StarNight.png

    type: Weapon
    uses: ammo Arrows

    damage: 14
    damage_class: Ranged
    knockback: 0
    critchance: 6
    usetime: 10
    velocity: 8
    tooltip: This weapon charges up when you shoot. <br>When fully charged it will shoot 6 deadly stars at your enemies.
    sacrificeCountNeeded: 1

obtained: 
    type: loot
    from: 
        - name: Skyware Chest
          url: https://terraria.wiki.gg/wiki/Skyware_Chest
          icon: https://terraria.wiki.gg/images/c/c4/Skyware_Chest.png
          quantity: 1
          drop_rate: 1/4 (25%)

parent: Ranged weapons
grand_parent: Weapons
great_grand_parent: Items
---

# Starry Night
---
{% include components/stats-item.html %}
{% include components/obtained_from.html %}