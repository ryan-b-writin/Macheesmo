using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
using MoreMountains.Tools;

public class BossEndLevelScript : MonoBehaviour
{
    [SerializeField] string LevelName;
    [SerializeField] float DelayBeforeTransition = 1f;
    [SerializeField] GameObject levelEnd;
    [SerializeField] Transform goalPosition;

    private void OnDestroy() {
        Instantiate(levelEnd,goalPosition);
    }

    void Update(){
        if(gameObject.active == false){
            Instantiate(levelEnd,goalPosition);
        }
    }

     public virtual void GoToNextLevel()
	    {
	    	if (LevelManager.HasInstance)
	    	{
                CorgiEngineEvent.Trigger(CorgiEngineEventTypes.LevelComplete);
			    MMGameEvent.Trigger("Save");
				LevelManager.Instance.GotoLevel(LevelName, (DelayBeforeTransition == 0f));
	    	}
	    	else
	    	{
                CorgiEngineEvent.Trigger(CorgiEngineEventTypes.LevelComplete);
			    MMGameEvent.Trigger("Save");
		        MMSceneLoadingManager.LoadScene(LevelName);
			}
	    }
}
