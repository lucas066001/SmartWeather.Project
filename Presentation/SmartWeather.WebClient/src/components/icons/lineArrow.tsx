import { twMerge } from "tailwind-merge";

interface ILineArrowProps {
    up: boolean;
    className? : string
}

function LineArrow({ className , up }: ILineArrowProps){
    return (
        <svg  className={twMerge(
            className,
            "transition-all",
            !up
                ? " "
                : "rotate-180 "
            )}
            width="14" height="9" viewBox="0 0 14 9" fill="none" xmlns="http://www.w3.org/2000/svg">
            <path d="M13 1L7 8L1 1" stroke="#B1B1B1" strokeWidth="1.75" stroke-linecap="round" stroke-linejoin="round"/>
        </svg>
    )
}

export default LineArrow;