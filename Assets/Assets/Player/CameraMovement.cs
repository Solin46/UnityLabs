using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform target; // ссылка на игрока
    [SerializeField] private float speedOfCamera = 2f; // величина оставания камеры от движения игрока
    [SerializeField] private Vector3 offset = new Vector3(0f, 8f, -6f); // координаты смещения камеры относительно игрока

    private void Start()
    {
        Vector3 targetInitialPosition = target.position + offset; // положение камеры в момент запуска (координаты игрока + смещение камеры)
        transform.position = targetInitialPosition; // ставит камеру в начальную точку
        transform.LookAt(target.position); // фиксирует угол наклона камеры на игрока
    }

    private void LateUpdate()
    {
        if (target == null) return; // защита от ошибки прикрепления игрока в Инспекторе

        Vector3 desiredPosition = target.position + offset; // пересчёт координат конечного положения камеры относительно перемещения игрока

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, speedOfCamera * Time.deltaTime); // LERP - рассчёт промежуточного положения камеры для плавности

        transform.position = smoothedPosition; // присвоение камеры промежуточных координат
    }
}