# Unianio Framework

**UNI**-ty

**ANI**-mation

**O**-rganizer

Unianio is unity framework for **procedural 3D humanoid character animations**

In addition to that, the framework provides:

 * Messaging
 * Inversion of Control
 * 3D Geometry and Math Utilities
 
## How to Install Unianio?

1. Download unity package: 
https://github.com/vbodurov/unianio-framework/raw/develop/unity-packages/Unianio%20Framework%20v0.9.0.unitypackage

2. Import it in your unity project by clicking on 

	**Menu - Assets - Import Package - Custom Package**

	then choose previously downloaded package and import it
	
3. Create new game object by right clicking on the scene in Hierarchy, then choosing **Game Object - Create Empty**

4. Then click on that object andf in the Inspector choose Add Component then choose Unianio Setup

## How to Import New Models?

Currently Unianio supports [Make Human](http://www.makehumancommunity.org/) models

You can create them as you like, there are only 2 requirements in Make Human:

1. In Pose/Animate you have to choose **Default no toes**

2. When you export the model (File - Export) choose **meter as unit scale**

Then inside Unity

1. After you add the model to the scene click on it and add **Unianio Make Human Model** from the inspector

2. You may want to change Persona ID giving a model name if you will have multiple models

