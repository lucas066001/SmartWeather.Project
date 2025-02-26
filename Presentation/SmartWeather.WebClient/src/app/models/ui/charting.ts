export interface TimeSerie {
    name: string,
    color: string,
    data: { x: Date; y: number }[]
}