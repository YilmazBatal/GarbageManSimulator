#### **TrashCollector**.**cs (Vehicle Storage)

###### -->  Class *TrashCollector*
	int vehicleCapacity 
	List<TrashSlot> vehicleInventory
##### How does *AddFromBin()* works?
	AddFromBin(List<TrashSlot> binInventory){
		Checks every `TrashSlot` in Bin's inventory
		e.g. --> (Plastic 5, Glass 3)
		
	    // Is there enough space
		Meanwhile checks if there is enough space left.
		If there is not enough space it breaks the method.
		
		// Decide how much to take depending on space left
		If there is enough space, it checks how many it can
		take via MathF.Min(spaceLeft, slot.amount)
		And then it stores the value as transferable
		
		// Adding to Vehicle Inventory
		If there is existing type of the trash in inventory
			It just increments with += `transferable`
		--- Otherwise 
		it adds TrashSlot to the List, adds `transferable`
		Lastly, Update vehicle trash count with transferable
		and remove the same amount from the bin
		
		///// Once the foreach loop ends, update HUD text 
	}
	
