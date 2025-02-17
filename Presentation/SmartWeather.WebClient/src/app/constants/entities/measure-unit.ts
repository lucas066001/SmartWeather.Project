export enum MeasureUnit {
    Unknown,
    Celsius,
    Percentage,
    UvStrength
}

export const MeasureUnitLabels: Map<number, string> = new Map([
    [MeasureUnit.Unknown, 'Inconnu'],
    [MeasureUnit.Celsius, '°C'],
    [MeasureUnit.Percentage, '%'],
    [MeasureUnit.UvStrength, 'Indice UV'],
]);