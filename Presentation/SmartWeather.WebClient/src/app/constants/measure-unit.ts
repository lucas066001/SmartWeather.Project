export enum MeasureUnit {
    Unknown,
    Celsius,
    Percentage,
    UvStrength
}

export const MeasureUnitLabels: { [key in MeasureUnit]: string } = {
    [MeasureUnit.Unknown]: 'Inconnu',
    [MeasureUnit.Celsius]: '°C',
    [MeasureUnit.Percentage]: '%',
    [MeasureUnit.UvStrength]: 'Indice UV'
};
