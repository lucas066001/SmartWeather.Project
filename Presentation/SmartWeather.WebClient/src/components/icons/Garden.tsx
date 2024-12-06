import React from "react";
import { IIcon } from "./type";

function Garden({className}: IIcon) {

  return (
    <svg
      width="25"
      height="26"
      viewBox="0 0 25 26"
      fill="none"
      xmlns="http://www.w3.org/2000/svg"
      className={className}
    >
      <path
        d="M6.66668 8.9841V8.13592C6.66668 7.01116 7.10567 5.93247 7.88707 5.13715C8.66847 4.34183 9.72828 3.89502 10.8333 3.89502C11.9384 3.89502 12.9982 4.34183 13.7796 5.13715C14.561 5.93247 15 7.01116 15 8.13592V17.9629L18.9417 13.951L18.3633 11.7543L18.3617 11.7441H18.36C18.3127 11.5635 18.3254 11.3721 18.396 11.1996C18.4666 11.027 18.5912 10.8831 18.7504 10.79C18.9096 10.697 19.0946 10.6601 19.2766 10.6851C19.4586 10.7101 19.6273 10.7955 19.7567 10.9281L23.09 14.3209C23.2197 14.4524 23.3032 14.6237 23.3276 14.8084C23.3521 14.993 23.3161 15.1808 23.2252 15.3425C23.1344 15.5043 22.9937 15.6311 22.825 15.7034C22.6563 15.7757 22.4688 15.7894 22.2917 15.7424L22.275 15.7373L20.12 15.1504L15 20.3616V20.8586C15 21.3085 14.8244 21.74 14.5119 22.0581C14.1993 22.3763 13.7754 22.555 13.3333 22.555H8.33334C7.89132 22.555 7.46739 22.3763 7.15483 22.0581C6.84227 21.74 6.66668 21.3085 6.66668 20.8586V20.0715L2.88668 16.2242C2.10626 15.4282 1.66797 14.3495 1.66797 13.225C1.66797 12.1005 2.10626 11.0219 2.88668 10.2258C3.27367 9.83206 3.73307 9.51972 4.23867 9.30666C4.74426 9.09361 5.28613 8.984 5.83334 8.9841H6.66668ZM6.66668 10.6805H5.83334C5.33873 10.6802 4.85516 10.8293 4.44383 11.1089C4.03249 11.3885 3.71189 11.786 3.52258 12.2511C3.33326 12.7162 3.28375 13.228 3.38031 13.7217C3.47687 14.2155 3.71515 14.669 4.06501 15.0248L6.66668 17.6712V10.6805ZM13.3333 8.9841V8.13592C13.3333 7.46107 13.07 6.81385 12.6011 6.33666C12.1323 5.85947 11.4964 5.59138 10.8333 5.59138C10.1703 5.59138 9.53442 5.85947 9.06558 6.33666C8.59674 6.81385 8.33334 7.46107 8.33334 8.13592V8.9841H13.3333Z"
      />
    </svg>
  );
}

export default Garden;
