+------------------+
| Electrical Bolt  |
+------------------+


Description:
-----------
A simple interface to make a LineRenderer vibrate and wiggle when connected to two transforms like a bolt of electricity.

How to Use:
----------

Made with Unity 2018.1.0f2. Probably works in older versions, but was not tested with them.

1) Create an empty Game Object called MyBolt
2) Add a Line Renderer and Audio Source with loopable electric-ish sound to MyBolt
3) Assign a material to the Line Renderer and adjust the width (0.1 is an okay place to start)
4) Attach ElectricalBolt (this script) and BoltConnector script to MyBolt
5) Create or find two other game objects that will be the start and end points of the bolt
6) In the BoltConnector script, assign the end points to Start Transform and End Transform
7) Run the scene and press <space>

Modify the BoltConnector script as required for your project.

See example scene for further demo details


License Terms:
-------------
Released under the MIT license. Do whatever you want with it.


Scary Legal Jargon:
------------------
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.


Have fun,
Richard
