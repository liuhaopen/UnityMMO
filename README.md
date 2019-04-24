# Usage  
var runner = gameObject.AddComponent<Cocos.ActionRunner>();  
var moveAction = Cocos.MoveBy.CreateLocal(1, new Vector3(50,30,40));   
var action = Cocos.Sequence.Create(moveAction, Cocos.DelayTime.Create(1), Cocos.FadeIn.Create(0.5));  
runner.PlayAction(action);   
See more details in the TestActions.cs file  
# Test  
create a cube and add a TestActions Component for it, then run the game:  
![image](https://github.com/liuhaopen/ReadmeResources/blob/master/UnityCocosAction/test_actions.gif)  