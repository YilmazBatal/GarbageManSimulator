#### **TrashBin**.cs (Trash Storage Equipment)

###### --> Class *TrashSlot*
	TrashType trashType 
	int amount
###### --> Class *TrashBin*
	 List<TrashSlot> inventory
	 TrashCollector trashCollector
##### How does *TrashBin.cs* transfer items to ***Vehicle Storage***

We need 4 conditions to met
- Does Player looking at vehicle?
- Is Bin beside the player?`
- Does player request the deposit?
 - Is there enough space in the vehicle? // If here is not enough space send a toast msg to player
Once all of these condition are met
- Run `AddFromBin(inventory)` //  This function is in TrashCollector/Vehicle
- Clear slots with the amount 0
- Update animation depending on isFull state,
- Update UI,
- Play SFX of transferring trash.