using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class NewTestScript
{
    // A Test behaves as an ordinary method

    // A UnityTest behaves like aa coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator PlayerDealsDamage()
    {
        var trash = Object.Instantiate(Resources.Load<TrashMonster>("Prefabs/TrashMonster 1"),
            Vector3.zero, Quaternion.identity);
        var player = Object.Instantiate(Resources.Load<Player>("Prefabs/Player"),
            Vector3.zero, Quaternion.identity);
        var playerType = typeof(Player);
        var startTrashHealth = trash.hp;

        trash.gameObject.layer = 9;
        playerType.GetMethod("OnAttack", BindingFlags.NonPublic | BindingFlags.Instance)
            .Invoke(player, new object[0]);
        yield return new WaitForSeconds(0.3f);
        Assert.Greater(startTrashHealth, trash.hp);
        
        GameObject.Destroy(player.gameObject);
        GameObject.Destroy(trash.gameObject);
    }

    [UnityTest]
    public IEnumerator TrashDealsDamage()
    {
        var trash = Object.Instantiate(Resources.Load<TrashMonster>("Prefabs/TrashMonster 1"),
            Vector3.zero, Quaternion.identity);
        trash.homePoint = new GameObject().transform;
        var player = Object.Instantiate(Resources.Load<Player>("Prefabs/Player"),
            Vector3.zero, Quaternion.identity);
        var startPlayerHp = player.health;
        trash.gameObject.layer = 9;

        yield return new WaitForSeconds(2f);
        Assert.Greater(startPlayerHp, player.health);
        GameObject.Destroy(player.gameObject);
        GameObject.Destroy(trash.gameObject);
    }

    [UnityTest]
    public IEnumerator PlayerRolls()
    {
        var player = Object.Instantiate(Resources.Load<Player>("Prefabs/Player"),
            new Vector3(0, 2), Quaternion.identity);
        var playerType = typeof(Player);
        
        Object.Instantiate(Resources.Load("Prefabs/platform (1)"), Vector3.zero, Quaternion.identity);
        yield return new WaitForSeconds(2f);
        var startPlayerPos = player.transform.position;
        playerType.GetMethod("Roll", BindingFlags.NonPublic | BindingFlags.Instance)
            .Invoke(player, new object[0]);
        yield return new WaitForSeconds(1f);
        Assert.Greater(startPlayerPos.x, player.transform.position.x);
        
        GameObject.Destroy(player.gameObject);
    }

    [UnityTest]
    public IEnumerator KoldunTeleports()
    {
        var player = Object.Instantiate(Resources.Load<Player>("Prefabs/Player"),
            new Vector3(0, 2), Quaternion.identity);
        Object.Instantiate(Resources.Load("Prefabs/platform (1)"), Vector3.zero, Quaternion.identity);
        yield return new WaitForSeconds(2f);
        var koldun = Object.Instantiate(Resources.Load<Koldun>("Prefabs/Koldun"),
            new Vector3(-100, -100), Quaternion.identity);
        koldun.player = player.transform;
        koldun.playerPosition = player.supportPosition;
        koldun.isFollowPlayer = true;
        koldun.groundCheck = player.groundCheck;
        var koldunStartPos = koldun.transform.position;
        yield return new WaitForSeconds(1f);
        Assert.AreEqual(player.transform.position.y, koldun.transform.position.y, 1);
        Assert.AreEqual(player.transform.position.x, koldun.transform.position.x, 1);
        
        GameObject.Destroy(player.gameObject);
        GameObject.Destroy(koldun.gameObject);
    }

    [UnityTest]
    public IEnumerator KoldunCreatesMagicBall()
    {
        var koldun = Object.Instantiate(Resources.Load<Koldun>("Prefabs/Koldun"),
            Vector3.zero, Quaternion.identity);
        koldun.Attack(Vector3.zero);
        yield return new WaitForSeconds(1f);
        Assert.NotNull(Object.FindObjectOfType<magic_ball_script>());
        
        GameObject.Destroy(koldun.gameObject);
    }

    [UnityTest]
    public IEnumerator ArcherAttacks()
    {
        var player = Object.Instantiate(Resources.Load<Player>("Prefabs/Player"),
            new Vector3(0, 0), Quaternion.identity);
        var archer = Object.Instantiate(Resources.Load<Archer>("Prefabs/Archer"),
            Vector3.zero, Quaternion.identity);
        var trash = Object.Instantiate(Resources.Load<TrashMonster>("Prefabs/TrashMonster 1"),
            new Vector3(2, 0), Quaternion.identity);
        trash.homePoint = new GameObject().transform;
        var startTrashHp = trash.hp;
        yield return new WaitForSeconds(1f);
        Assert.Greater(startTrashHp, trash.hp);
        
        GameObject.Destroy(player.gameObject);
        GameObject.Destroy(archer.gameObject);
        GameObject.Destroy(trash.gameObject);
    }

    [UnityTest]
    public IEnumerator ArcherTeleports()
    {
        var player = Object.Instantiate(Resources.Load<Player>("Prefabs/Player"),
            new Vector3(0, 2), Quaternion.identity);
        Object.Instantiate(Resources.Load("Prefabs/platform (1)"), Vector3.zero, Quaternion.identity);
        yield return new WaitForSeconds(2f);
        var archer = Object.Instantiate(Resources.Load<Archer>("Prefabs/Archer"),
            new Vector3(-100, -100), Quaternion.identity);
        archer.player = player.transform;
        archer.playerPosition = player.supportPosition;
        archer.isFollowPlayer = true;
        archer.groundCheck = player.groundCheck;
        yield return new WaitForSeconds(1f);
        Assert.AreEqual(player.transform.position.y, archer.transform.position.y, 1);
        Assert.AreEqual(player.transform.position.x, archer.transform.position.x, 1);
        
        GameObject.Destroy(player.gameObject);
        GameObject.Destroy(archer.gameObject);
    }

    // [UnityTest]
    // public IEnumerator RandomWarriorDealsDamage()
    // {
    //     var player = Object.Instantiate(Resources.Load<Player>("Prefabs/Player"),
    //         new Vector3(0, 2), Quaternion.identity);
    //     var warrior = Object.Instantiate(Resources.Load<Random_warrior>("Prefabs/Random Warrior"),
    //         new Vector3(0, 2), Quaternion.identity);
    //     Object.Instantiate(Resources.Load("Prefabs/platform (1)"), Vector3.zero, Quaternion.identity);
    //     // Object.Instantiate(Resources.Load("Prefabs/platform (1)"), new Vector3(-0.1f, 0), Quaternion.identity);
    //     var startPlayerHp = player.health;
    //     player.gameObject.layer = 10;
    //     warrior.gameObject.layer = 9;
    //     warrior.homePoint = new GameObject().transform;
    //     yield return new WaitForSeconds(4f);
    //     Debug.Log(player.health);
    //     Assert.Greater(startPlayerHp, player.health);
    //     
    //     Object.Destroy(player);
    //     Object.Destroy(warrior);
    // }
}
