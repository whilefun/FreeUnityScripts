===================================================
> While Fun Games - Terrain Object Placement Tool <
===================================================

------------
> Overview <
------------

A simple editor window that allows objects to be automatically places onto the surface of a Terrain. This allows for topology to be changed over time and easy put everything back on top with one button click.


---------
> Usage <
---------

Setup:

1) Place the TerrainObjectPlacementTool folder into your project's Assets folder.
2) To make your existing or new scene GameObjects or Prefabs work with automatic object placement, add the included WFGTerrainObject script to them.

To use the tool:

1) Open the Editor Window using the While Fun Games menu, and select Terrain Object Placement Tool.
2) In the editor window, assign the Terrain object into the Selected Terrain field, or click the Auto Find Terrain button.
3) Place your objects on the terrain or at any XZ position that overlaps with a terrain. 
4) To put all WFGTerrainObjects on top of the terrain, simply click the "Refresh Placement on Terrain" button.

Objects should now be on top of the terrain.

Optionally, you can set a Vertical Offset for your WFGTerrainObjects in case they are vertically not centered. For example, if you have some root that is meant to descend into the terrain a little bit, you can make the vertical offset negative to bury the object into the terrain slightly, relative to the objects origin.


-----------
> License <
-----------

Copyright (c) 2022 While Fun Games

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
