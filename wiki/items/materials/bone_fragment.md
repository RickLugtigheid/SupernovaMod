---
layout: default
title: Bone Fragment

item:
    name: Bone Fragment
    icon: https://raw.githubusercontent.com/KoekMeneer/SupernovaMod/main/Content/Items/Materials/BoneFragment.png

    type: Material
    tooltip: Drops from any zombies after the Brain of Cthulhu OR Eater of Worlds is defeated.
    sacrificeCountNeeded: 25

obtained: 
    type: npc
    from: 
        - name: Any Zombe
          url: https://terraria.wiki.gg/wiki/Zombie
          icon: https://terraria.wiki.gg/images/c/c3/Zombie.png
          quantity: 1-3
          drop_rate: 1/7 (14%)

parent: Materials
grand_parent: Items
---

# Bone Fragment
---
{% include components/stats-item.html %}
{% include components/obtained_from.html %}

## Crafting
---
### Used in
{% include components/recipe.html recipe=site.data.recipes.carnage_rifle %}