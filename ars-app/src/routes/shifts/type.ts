export type ShiftsDto = {
    id: number;
    shftCode: string;
    shftDesc?: string | null;
    shftTpolicyCode: string;
    shftType?: string | null;
    shftMswipe: number;
    shftRegHrs: number;
    shftMandatorySwipe: number;
    shftTmIn1?: string | null;
    shftTmOut1?: string | null;
    shftTmIn2?: string | null;
    shftTmOut2?: string | null;
    shftTmIn3?: string | null;
    shftTmOut3?: string | null;
    shftTmIn4?: string | null;
    shftTmOut4?: string | null;
    shftCreaBy?: string | null;
    shftCreaDt?: Date | null;
    shftModiBy?: string | null;
    shftModiDt?: Date | null;
}

export type TimePolicyCode = {
    tpolCode: string;
    tpolDesc?: string | null;
};