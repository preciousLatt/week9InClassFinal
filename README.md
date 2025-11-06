# Road Fighter


We implemented a Fuel pick object and a fuel meter for the player and the fuel objects are pooled and spawned when needed. 
We planned on adding a UI element to display the current fuel, and to avoid pointless updates we would use a dirty flag to update the UI only when the fuel has changed (either by player driving or fuel packs getting picked up) but due to time constraints we couldn't as we didn’t have a starting baseline from last week.
