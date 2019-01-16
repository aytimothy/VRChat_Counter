# VRChat_Counter
A simple counter in VRChat that is network-friendly.

# How do I integrate it into my own world?
What you'll want to do is copy the entirety of the `Logic` folder over to your other project.  

If you don't want fonts to break, don't forget `Fonts/DS-DIGI.TTF` as well.
Next, setup your array of digits that will make up your scoreboard.

Finally, for every digit, from right to left (except the left-most), configure:

  * The `Underflow` event to trigger the animator trigger `decrement` on the animator (digit) to the left.
  * The `Overflow` event to trigger the animator trigger `increment` on the animator (digit) to the left.
  
To increment or decrement, you can just call the animator trigger `increment` or `decrement` using another `VRC_Trigger` attached to a button, somewhere else.  
To set the values for a digit, just set `sync_state` to whichever digit you want the digit to be, and then trigger animator trigger `sync` using another `VRC_Trigger` attached to a button, somewhere else.

# Credits
Credit me, though I don't want you to just go pasting the GNU License into your world.

# Third-party Content

  * Stone Button - Minecraft, Mojang AB
  * Network Theme - Megaman Star Force 2, Capcom
  * Various Clock Icons - Flaticon
  * DS Digital - Dusit Supasawat [(Dafont)](https://www.dafont.com/ds-digital.font)
  * Lilita One - Juan Montoreano [(Google Fonts)](https://fonts.google.com/specimen/Lilita+One)
  
Obviously I never asked for any permission. I just cobbled this together with whatever I had, original or not.
