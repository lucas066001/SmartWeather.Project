export const PinoutLabels: Map<number, number> = new Map([
    [32, 1],  // GPIO 32 → Socket 1
    [34, 2],  // GPIO 34 → Socket 2
    [18, 3],  // GPIO 18 → Socket 3
    [33, 4],  // GPIO 33 → Socket 4
]);

/**
 * Retreive socket number corresponding to gpioPin.
 * @param gpioPin GPIO pin number.
 * @returns Socket number or `null` if not found.
 */
export function getSocketNumber(gpioPin: number): number | null {
    return PinoutLabels.get(gpioPin) ?? null;
}
