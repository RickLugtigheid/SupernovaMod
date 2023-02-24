---
layout: default
title: Contributing
nav_exclude: true
permalink: /docs/contributing
---

# Contributing
{: .no_toc }
A guide for contributing to the Supernova Mod / Wiki.
{: .fs-6 .fw-300 }

## Table of contents
{: .no_toc .text-delta }

1. TOC
{:toc}

---

## Contributing to the Wiki

### Table of content
 - [Required knowledge](#required-knowledge)
 - [How to: Edit a wiki page](#how-to-edit-a-wiki-page)
 - [How to: Add a wiki page](#how-to-add-a-wiki-page)
 - [How to: Add a crafting recipe](#how-to-add-a-crafting-recipe)
 - [How to: Add a obtained from table](#how-to-add-a-obtained-from-table)

### Required knowledge:
##### For Basic edits
 - Basic [Git](https://git-scm.com/)/[Github](https://github.com/)
 - [Markdown](https://www.markdownguide.org/)
##### For Advanced edits
 - Basic [Liquid/Jekyll](https://jekyllrb.com/)
 - [YAML](https://yaml.org/)
 - [JSON](https://www.json.org/)

---

### How to: Edit a wiki page
There are 2 ways to edit a wiki page.
You can edit a page on Github or clone the project with Git and edit it in a text editor.

#### Edit on Github
The first and easy way is to navigate to a page (*This page for example*), scroll down to the bottom, and click the "Edit this page on GitHub" link.

When you do you will be redirected to github, from there you can click the "pen icon" to edit the page.

{: .note }
> This action requires that you have a [Github Account](https://github.com/join)

#### Edit on local device
(Advanced): The second way is to edit the page on a local device (*like a computer or laptop*).

For this you will need to clone the project:
```shell
$ git clone https://github.com/KoekMeneer/SupernovaMod.git
```

Checkout the "gh-pages-dev" brach:
```shell
$ git checkout gh-pages-dev
```

Make your changes (*with a text editor of choice*), and commit your changes (*Add a commit message with your changes*):
```shell
$ git commit -m "Change ... and ... on the .. page"
```

---

### How to: Add a wiki page
If you want to add a new page, for example a page about an item added in the newest update you will need to follow the following steps.

#### 1. Clone the project repository
```shell
$ git clone https://github.com/KoekMeneer/SupernovaMod.git
```

#### 2. Checkout the "gh-pages-dev" brach
```shell
$ git checkout gh-pages-dev
```

#### 3. Add a page
Go to the project folder, open the wiki folder and navigate to the place you want to make a new page (*For example: items/materials*), and create a new Markdown file (*A file with the .md extension*) with the page name in lower_case.

After this you can add some content to the page.
Example of a simple page:
```md
---
layout: default
title: My Page
parent: Materials
grand_parent: Items
---

# My page
---
This is my page...
```

#### 4. Commit your new page
```shell
$ git commit -m "Change ... and ... on the .. page"
```

---

### How to: Add a crafting recipe

{: .warning }
> No documentation written for this yet

---

### How to: Add a obtained from table

{: .warning }
> No documentation written for this yet

---