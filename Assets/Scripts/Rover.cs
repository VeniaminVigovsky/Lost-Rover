using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rover : MonoBehaviour
{
    List<PathNode> nodeList = new List<PathNode>();
    RoverEnergy roverEnergy;
    bool fallen = false;
    bool reachedEnding = false;
    bool onRechargePoint = false;
    RechargePoint rechargePoint;
    float movementPace = 0.2f;

    RoverAudioManager roverAudio;
    Animator animator;

    [SerializeField]
    MapGenerator mapGenerator;

    public delegate void EndingReachedHandler();
    public static event EndingReachedHandler EndingReached;

    public delegate void RoverDeathHandler();
    public static event RoverDeathHandler RoverDead;

    public delegate void RoverJustDeathHandler();
    public static event RoverJustDeathHandler RoverJustDead;

    private void Awake()
    {
        GameManager.RegisterRover(this);
        GameManager.UnlockInput();
        ClearNodeList();
        if (mapGenerator != null)
        {
            mapGenerator.GenerationEnded += SetStartingPosition;
        }

    }

    private void OnEnable()
    {
        if (mapGenerator != null)
        {
            mapGenerator.GenerationEnded += SetStartingPosition;
        }

        EndingReached += OnEndingReached;
        
    }
    private void OnDisable()
    {
        EndingReached -= OnEndingReached;
        roverEnergy.EnergyDrained -= DeathOfEnergyDrain;
        if (mapGenerator != null)
        {
            mapGenerator.GenerationEnded -= SetStartingPosition;
        }
    }

    private void OnDestroy()
    {
        EndingReached -= OnEndingReached;
        roverEnergy.EnergyDrained -= DeathOfEnergyDrain;

        if (mapGenerator != null)
        {
            mapGenerator.GenerationEnded -= SetStartingPosition;
        }
    }

    private void Start()
    {
        transform.position = new Vector3(-20f, 20f, 0f);
        roverEnergy = GetComponent<RoverEnergy>();
        roverEnergy.EnergyDrained += DeathOfEnergyDrain;
        fallen = false;
        reachedEnding = false;
        onRechargePoint = false;
        animator = GetComponentInChildren<Animator>();
        animator.Play("Rover_Idle");
        roverAudio = GetComponentInChildren<RoverAudioManager>();
    }
    string currentDirection = "South";

    public void ClearNodeList()
    {
        nodeList.Clear();
    }

    public void AddToNodeList(PathNode node)
    {        

        if (nodeList.Count > 0)
        {
            var vectorToNewNode = nodeList[nodeList.Count - 1].transform.position - node.transform.position;
            vectorToNewNode.Normalize();            
            if (vectorToNewNode.x != 0 && vectorToNewNode.y != 0)
            {
                if (IsInTheNodeList(node))
                {
                    if (nodeList[nodeList.Count - 1] == node)
                        return;
                    else
                    {
                        while (nodeList[nodeList.Count - 1] != node)
                        {
                            nodeList[nodeList.Count - 1].SetSprite(null);
                            nodeList.RemoveAt(nodeList.Count - 1);
                        }     
                    }
                }
                else
                {
                    node.SetSprite(null);
                    
                }
                
            }
            else
            {
                if (IsInTheNodeList(node))
                {
                    if (nodeList[nodeList.Count - 1] == node)
                        return;
                    else
                    {
                        while (nodeList[nodeList.Count - 1] != node)
                        {
                            nodeList[nodeList.Count - 1].SetSprite(null);
                            nodeList.RemoveAt(nodeList.Count - 1);
                        }

                    }


                }
                else
                {
                    nodeList.Add(node);
                }
            }

        }
        else
        {
            nodeList.Add(node);
        }

        
    }

    public bool IsInTheNodeList(PathNode node)
    {
        return nodeList.Contains(node);
    }

    public void StartMovement()
    {
        if (nodeList.Count < 2)
        {
            foreach (var node in nodeList)
            {
                node.SetSprite(null);
            }
            ClearNodeList();

            return;
        }
        StopAllCoroutines();
        StartCoroutine(Movement());
       
    }

    private IEnumerator Movement()
    {
        GameManager.LockInput();
        roverAudio.StartMoving();
        for(int i = 0; i < nodeList.Count; i++)
        {
                      

            while (Vector2.Distance(transform.position, nodeList[i].transform.position) > movementPace)
            {
                transform.position = Vector2.MoveTowards(transform.position, nodeList[i].transform.position, movementPace);


                yield return new WaitForSeconds(0.08f);
            }

            transform.position = nodeList[i].transform.position;
            nodeList[i].SetSprite(null);
            yield return null;
            

            if (reachedEnding)
            {
                EndingReached?.Invoke();
                roverAudio.RoverFullStop();
                yield break;
            }

            //Rotation
            if (i < nodeList.Count - 1)
            {

                if (nodeList[i].transform.position.x < nodeList[i + 1].transform.position.x && Mathf.Abs(nodeList[i].transform.position.x - nodeList[i + 1].transform.position.x) > 1 && Mathf.Abs(nodeList[i].transform.position.y - nodeList[i + 1].transform.position.y) < 1)
                {                                       

                    if (currentDirection != "East")
                    {
                        if (currentDirection == "South")
                        {
                            for (int j = 0; j < 6; j++)
                            {
                                transform.Rotate(0f, 0f, 15f);
                                yield return new WaitForSeconds(0.2f);
                            }

                        }
                        else if (currentDirection == "North")
                        {
                            for (int j = 0; j < 6; j++)
                            {
                                transform.Rotate(0f, 0f, -15f);
                                yield return new WaitForSeconds(0.2f);
                            }
                        }

                        else if (currentDirection == "West")
                        {
                            for (int j = 0; j < 12; j++)
                            {
                                transform.Rotate(0f, 0f, -15f);
                                yield return new WaitForSeconds(0.2f);
                            }
                        }

                        transform.eulerAngles = new Vector3(0f, 0f, 90f);
                    }

                    currentDirection = "East";
                    //turn East


                }
                else if (nodeList[i].transform.position.x > nodeList[i + 1].transform.position.x && Mathf.Abs(nodeList[i].transform.position.x - nodeList[i + 1].transform.position.x) > 1 && Mathf.Abs(nodeList[i].transform.position.y - nodeList[i + 1].transform.position.y) < 1)
                {
                    //Turn West                    
           
                    if (currentDirection != "West")
                    {
                        if (currentDirection == "South")
                        {
                            for (int j = 0; j < 6; j++)
                            {
                                transform.Rotate(0f, 0f, -15f);
                                yield return new WaitForSeconds(0.2f);
                            }

                        }
                        else if (currentDirection == "North")
                        {
                            for (int j = 0; j < 6; j++)
                            {
                                transform.Rotate(0f, 0f, 15f);
                                yield return new WaitForSeconds(0.2f);
                            }
                        }

                        else if (currentDirection == "East")
                        {
                            for (int j = 0; j < 12; j++)
                            {
                                transform.Rotate(0f, 0f, 15f);
                                yield return new WaitForSeconds(0.2f);
                            }
                        }
                    }

                    currentDirection = "West";


                }
                else if (nodeList[i].transform.position.y < nodeList[i + 1].transform.position.y && Mathf.Abs(nodeList[i].transform.position.y - nodeList[i + 1].transform.position.y) > 1 && Mathf.Abs(nodeList[i].transform.position.x - nodeList[i + 1].transform.position.x) < 1)
                {                    
                    
                    if (currentDirection != "North")
                    {
                        if (currentDirection == "West")
                        {
                            for (int j = 0; j < 6; j++)
                            {
                                transform.Rotate(0f, 0f, -15f);
                                yield return new WaitForSeconds(0.2f);
                            }

                        }
                        else if (currentDirection == "East")
                        {
                            for (int j = 0; j < 6; j++)
                            {
                                transform.Rotate(0f, 0f, 15f);
                                yield return new WaitForSeconds(0.2f);
                            }
                        }

                        else if (currentDirection == "South")
                        {
                            for (int j = 0; j < 12; j++)
                            {
                                transform.Rotate(0f, 0f, 15f);
                                yield return new WaitForSeconds(0.2f);
                            }
                        }
                        //turn North                        
                        transform.eulerAngles = new Vector3(0f, 0f, 180f);
                    }

                    currentDirection = "North";


                }
                else if (nodeList[i].transform.position.y > nodeList[i + 1].transform.position.y && Mathf.Abs(nodeList[i].transform.position.y - nodeList[i + 1].transform.position.y) > 1 && Mathf.Abs(nodeList[i].transform.position.x - nodeList[i + 1].transform.position.x) < 1)
                {                    
                    
                    if (currentDirection != "South")
                    {
                        if (currentDirection == "West")
                        {
                            for (int j = 0; j < 6; j++)
                            {
                                transform.Rotate(0f, 0f, 15f);
                                yield return new WaitForSeconds(0.2f);
                            }

                        }
                        else if (currentDirection == "East")
                        {
                            for (int j = 0; j < 6; j++)
                            {
                                transform.Rotate(0f, 0f, -15f);
                                yield return new WaitForSeconds(0.2f);
                            }
                        }

                        else if (currentDirection == "North")
                        {
                            for (int j = 0; j < 12; j++)
                            {
                                transform.Rotate(0f, 0f, -15f);
                                yield return new WaitForSeconds(0.2f);
                            }
                        }

                        transform.eulerAngles = new Vector3(0f, 0f, 0f);                        
                    }

                    currentDirection = "South";

                    //turn South

                }


            }



        }

        roverAudio.StopMoving();

        if (onRechargePoint && !rechargePoint.IsUsed())
        {
            ActivateRechargePoint();
            yield break;
        }

        roverEnergy.UseEnergyCell();

        

        ClearNodeList();        
        GameManager.UnlockInput();       
               
    }

    private void DeathOfEnergyDrain()
    {
        StopCoroutine(Movement());
        roverAudio.EnergyDrained();
        ClearNodeList();
        GameManager.UnlockInput();
        RoverDead?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Pitfall"))
        {            
            DeathOfFalling();
            RoverJustDead?.Invoke();
        }
        else if (collision.CompareTag("EndingPoint"))
        {
            reachedEnding = true;
        }
        else if (collision.CompareTag("RechargePoint"))
        {
            rechargePoint = collision.GetComponentInParent<RechargePoint>();

            onRechargePoint = true;

            
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("RechargePoint"))
        {
            onRechargePoint = false;
        }
    }

    private void DeathOfFalling()
    {
        
        StopAllCoroutines();
        foreach (var node in nodeList)
        {
            node.SetSprite(null);

        }
        ClearNodeList();
        roverAudio.RoverFallen();
        animator.Play("Rover_Fall");

    }

    private void OnEndingReached()
    {
        StopCoroutine(Movement());
        roverAudio.StopMoving();
        Score.UpdateScore(Score.currentScore + 10);
        foreach (var node in nodeList)
        {
            node.SetSprite(null);
        }
        ClearNodeList();
        
    }

    private void ActivateRechargePoint()
    {

        rechargePoint.UseRechargePoint();
        StopCoroutine(Movement());        
        ClearNodeList();
        roverEnergy.RefillEnergy();
        GameManager.UnlockInput();


    }


    public void SetStartingPosition(Vector3 eventContext)
    {
        transform.position = mapGenerator.GetStartingPosition();
    }

    public void RoverFallen()
    {
        roverEnergy.SetEnergy(0);
        GameManager.UnlockInput();
        RoverDead?.Invoke();
    }

}
