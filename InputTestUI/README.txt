+----------------+
| Input Debugger |
+----------------+


Description:
-----------
A simple set of scripts and prefabs to allow you to display realtime input data on screen to debug your game.


How to Use:
----------
1) Put the InputDebugger folder into your Assets folder.
2) Open the Assets\InputDebugger\Scenes\InputDebuggerExample scene, and run it
3) Wiggle the mouse, and press the Spacebar and C keys to see the debugger in action
4) Optionally, press 'T' to toggle the debugger on and off.


Modifications:
-------------
Note that depending on your input library of choice, you will need to edit the scripts. Specifically, InputDebugger.Update() must fetch values from your input library to refresh the UI states. There is a (very hasty and bad) InputManager included for demonstration purposes. You probably want to replace this and use something like Rewired, or create your own more feature complete version of this script instead. 

For details on how to use with Rewired, see Assets\InputDebugger\Documentation\HowToUseWithRewired.txt



License Terms:
-------------
Released under the MIT license. Do whatever you want with it.


Scary Legal Jargon:
------------------
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
