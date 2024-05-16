using UnityEngine;

public class SkillCoinPickUp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // if the other is the player.
        if(other.gameObject.GetComponent<PlayerMovement>() != null)
        {
            // then add the skill coin
            Game.gameSingleton.stateMgr.pickUpSkillCoins(1);

            // and destroy itself
            // (note that the collider is a subobject of the whole coin)
            // so actually I need to destroy its parent.
            GameObject.Destroy(gameObject.transform.parent.gameObject);

            // Finally add another skill coin
            Game.gameSingleton.mapMgr.createSkillCoin();
        }
    }
}